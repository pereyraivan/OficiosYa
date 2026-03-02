using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OficiosYa.Web.Services;

/// <summary>
/// Servicio que usa la API de moderación de OpenAI (omni-moderation-latest)
/// para detectar contenido violento, sexual u ofensivo en texto e imágenes.
/// Requiere API key en configuración: OpenAI:ApiKey. Si no está configurada, no se modera.
/// </summary>
public interface IOpenAIModerationService
{
    /// <summary>Indica si el servicio está disponible (tiene API key).</summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Revisa textos e imágenes. Si algún contenido es detectado como inapropiado (violencia, sexual, etc.),
    /// devuelve false y un mensaje para el usuario. Si todo está bien, devuelve (true, null).
    /// </summary>
    Task<(bool Aprobado, string? MensajeRechazo)> ModerateAsync(
        IEnumerable<string>? textos = null,
        IEnumerable<string>? imagenesBase64 = null,
        CancellationToken cancellationToken = default);
}

public class OpenAIModerationService : IOpenAIModerationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<OpenAIModerationService>? _logger;
    private readonly IImageModerationService? _imageModeration;
    private const string ModerationUrl = "https://api.openai.com/v1/moderations";
    private const string Model = "omni-moderation-latest";
    /// <summary>Pausa entre requests para no superar el rate limit de OpenAI (máx 1 imagen por request = varios requests seguidos).</summary>
    private static readonly TimeSpan DelayEntreRequests = TimeSpan.FromMilliseconds(800);

    /// <summary>Palabras bloqueadas (insultos/profanidad, argentinos y generales). Se revisa siempre, con o sin API key.</summary>
    private static readonly HashSet<string> PalabrasBloqueadas = new(StringComparer.OrdinalIgnoreCase)
    {
        "mierda", "puto", "puta", "putos", "putas", "concha", "concho", "boludo", "boluda",
        "pelotudo", "pelotuda", "forro", "forra", "garca", "gil", "gila", "tonto", "tonta",
        "idiota", "estúpido", "estupido", "estúpida", "estupida", "carajo", "verga", "vergas",
        "coño", "coger", "joder", "cabrón", "cabron", "cabrona", "maricón", "maricon", "maricona",
        "negro", "negra", "judío", "judio", "judía", "judia", "violar", "violación", "violacion",
        "matar", "muerte", "odio", "asqueroso", "asquerosa", "basura", "pendejo", "pendeja",
        "chingar", "chingada", "chingado", "pinche", "puta madre", "hijo de puta", "hija de puta", "la puta", "qué mierda", "que mierda", "no mierda", "una mierda",
        "pingo", "pene", "penes", "culo", "culos", "vagina", "vaginas", "ura", "orto", "ortiva",
        "choto", "chota", "mogólico", "mogolica", "mogolico", "tarado", "tarada", "salame", "nabo", "nabos", "hdp"
    };

    public OpenAIModerationService(HttpClient httpClient, IConfiguration config, ILogger<OpenAIModerationService>? logger = null, IImageModerationService? imageModeration = null)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
        _imageModeration = imageModeration;
        var apiKey = _config["OpenAI:ApiKey"]?.Trim();
        if (!string.IsNullOrEmpty(apiKey))
        {
            // Asegurar que no se envíe "Bearer Bearer sk-..." si la key ya incluye "Bearer"
            if (apiKey.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                apiKey = apiKey.Substring(7).Trim();
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            _logger?.LogInformation("Moderación OpenAI: activa (API key configurada).");
        }
        else
            _logger?.LogInformation("Moderación OpenAI: inactiva (falta OpenAI:ApiKey en configuración o User Secrets).");
    }

    public bool IsEnabled => !string.IsNullOrWhiteSpace(_config["OpenAI:ApiKey"]);

    public async Task<(bool Aprobado, string? MensajeRechazo)> ModerateAsync(
        IEnumerable<string>? textos = null,
        IEnumerable<string>? imagenesBase64 = null,
        CancellationToken cancellationToken = default)
    {
        // 1. Filtro local de palabras bloqueadas (siempre activo, no depende de la API)
        if (textos != null)
        {
            foreach (var texto in textos)
            {
                if (string.IsNullOrWhiteSpace(texto)) continue;
                var palabraEncontrada = ContienePalabraBloqueada(texto.Trim());
                if (palabraEncontrada != null)
                {
                    _logger?.LogInformation("Moderación: contenido bloqueado por palabra no permitida.");
                    return (false, "El contenido no cumple las normas de la comunidad. No se permite lenguaje ofensivo o inapropiado.");
                }
            }
        }

        // 2. Imágenes: Sightengine (nudity, violence, gore). Si no está configurado, se permiten.
        if (_imageModeration != null && _imageModeration.IsEnabled && imagenesBase64 != null)
        {
            foreach (var base64 in imagenesBase64)
            {
                if (string.IsNullOrWhiteSpace(base64)) continue;
                var raw = base64.Trim();
                if (raw.Contains(',')) raw = raw.Substring(raw.IndexOf(',') + 1);
                byte[] bytes;
                try
                {
                    bytes = Convert.FromBase64String(raw);
                }
                catch
                {
                    continue;
                }
                var ext = raw.StartsWith("iVBOR", StringComparison.Ordinal) ? "png" : raw.StartsWith("/9j/", StringComparison.Ordinal) ? "jpg" : "png";
                var (aprobado, mensaje) = await _imageModeration.ModerateImageAsync(bytes, "image." + ext, cancellationToken);
                if (!aprobado) return (aprobado, mensaje);
            }
        }

        return (true, null);
    }

    /// <summary>Envía un único request a la API (solo textos, o una sola imagen).</summary>
    private async Task<(bool Aprobado, string? MensajeRechazo)> EnviarModeracionAsync(
        List<string>? textos,
        string? imagenDataUrl,
        CancellationToken cancellationToken)
    {
        object input;
        if (imagenDataUrl != null)
        {
            input = new[] { new { type = "image_url", image_url = new { url = imagenDataUrl } } };
        }
        else if (textos != null && textos.Count > 0)
        {
            if (textos.Count == 1)
                input = textos[0];
            else
                input = textos.Select(t => new { type = "text", text = t }).ToList();
        }
        else
            return (true, null);

        var requestBody = new { model = Model, input };
        var options = new JsonSerializerOptions { PropertyNamingPolicy = null, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        var jsonBody = JsonSerializer.Serialize(requestBody, options);
        const int maxIntentos = 2;
        for (var intento = 1; intento <= maxIntentos; intento++)
        {
            using var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(ModerationUrl, content, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests && intento < maxIntentos)
            {
                _logger?.LogWarning("OpenAI Moderation: Too Many Requests, reintento en 2s (intento {Intento})", intento);
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
                continue;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger?.LogWarning("OpenAI Moderation API error: {Status} {Body}", response.StatusCode, errorBody);
                return (false, ExtraerMensajeErrorOpenAI(errorBody, response.StatusCode));
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            return ProcesarRespuestaModeracion(json);
        }

        return (false, "Demasiadas solicitudes. Esperá un momento e intentá de nuevo.");
    }

    private (bool Aprobado, string? MensajeRechazo) ProcesarRespuestaModeracion(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("results", out var results))
                return (false, "No se pudo verificar el contenido. Por favor intentá de nuevo más tarde.");
            for (var i = 0; i < results.GetArrayLength(); i++)
            {
                var result = results[i];
                if (result.TryGetProperty("flagged", out var flagged) && flagged.GetBoolean())
                {
                    var categories = new List<string>();
                    if (result.TryGetProperty("categories", out var cat))
                    {
                        foreach (var prop in cat.EnumerateObject())
                            if (prop.Value.ValueKind == JsonValueKind.True && prop.Value.GetBoolean())
                                categories.Add(prop.Name);
                    }
                    var motivo = categories.Count > 0 ? string.Join(", ", categories) : "contenido inapropiado";
                    return (false, $"El contenido no cumple las normas de la comunidad (detección: {motivo}). No se permite contenido violento, sexual u ofensivo.");
                }
            }
        }
        catch (JsonException ex)
        {
            _logger?.LogError(ex, "OpenAI Moderation: respuesta JSON inválida.");
            return (false, "No se pudo verificar el contenido. Por favor intentá de nuevo más tarde.");
        }
        return (true, null);
    }

    /// <summary>Comprueba si el texto contiene alguna palabra bloqueada (por palabra completa).</summary>
    private static string? ContienePalabraBloqueada(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return null;
        var palabras = Regex.Split(texto, @"\W+");
        foreach (var p in palabras)
        {
            if (string.IsNullOrEmpty(p)) continue;
            if (PalabrasBloqueadas.Contains(p)) return p;
        }
        return null;
    }

    private static string ToDataUrl(string base64)
    {
        var prefix = "data:image/jpeg;base64,";
        if (base64.StartsWith("iVBOR", StringComparison.Ordinal))
            prefix = "data:image/png;base64,";
        else if (base64.StartsWith("R0lGO", StringComparison.Ordinal))
            prefix = "data:image/gif;base64,";
        else if (base64.StartsWith("/9j/", StringComparison.Ordinal))
            prefix = "data:image/jpeg;base64,";
        return prefix + base64;
    }

    /// <summary>Parsea el cuerpo de error de la API de OpenAI y devuelve un mensaje claro para el usuario.</summary>
    private static string ExtraerMensajeErrorOpenAI(string errorBody, System.Net.HttpStatusCode statusCode)
    {
        if (string.IsNullOrWhiteSpace(errorBody))
            return statusCode switch
            {
                System.Net.HttpStatusCode.Unauthorized => "Error de configuración con el servicio de moderación. Revisá que la API key de OpenAI sea correcta.",
                System.Net.HttpStatusCode.TooManyRequests => "Demasiadas solicitudes. Esperá un momento e intentá de nuevo.",
                _ => "No se pudo verificar el contenido. Por favor intentá de nuevo más tarde o usá otras imágenes."
            };
        try
        {
            using var doc = JsonDocument.Parse(errorBody);
            if (doc.RootElement.TryGetProperty("error", out var err) && err.TryGetProperty("message", out var msg))
            {
                var msgStr = msg.GetString() ?? "";
                if (msgStr.Contains("api_key", StringComparison.OrdinalIgnoreCase) || msgStr.Contains("Incorrect API key", StringComparison.OrdinalIgnoreCase))
                    return "La API key de OpenAI no es válida. Revisá la configuración (User Secrets o appsettings).";
                if (msgStr.Contains("rate_limit", StringComparison.OrdinalIgnoreCase) || msgStr.Contains("quota", StringComparison.OrdinalIgnoreCase))
                    return "Límite de uso alcanzado. Esperá un momento e intentá de nuevo.";
                if (msgStr.Length > 200) msgStr = msgStr.Substring(0, 200) + "...";
                return "No se pudo verificar el contenido: " + msgStr;
            }
        }
        catch { /* ignorar parseo */ }
        return statusCode switch
        {
            System.Net.HttpStatusCode.Unauthorized => "Error de configuración con el servicio de moderación. Revisá que la API key de OpenAI sea correcta.",
            System.Net.HttpStatusCode.TooManyRequests => "Demasiadas solicitudes. Esperá un momento e intentá de nuevo.",
            _ => "No se pudo verificar el contenido. Por favor intentá de nuevo más tarde o usá otras imágenes."
        };
    }
}
