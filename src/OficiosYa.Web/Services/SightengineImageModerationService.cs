using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace OficiosYa.Web.Services;

/// <summary>
/// Moderación de imágenes con Sightengine (nudity, violence, gore).
/// Configuración: Sightengine:ApiUser, Sightengine:ApiSecret.
/// </summary>
public interface IImageModerationService
{
    bool IsEnabled { get; }
    Task<(bool Aprobado, string? MensajeRechazo)> ModerateImageAsync(byte[] imageBytes, string? fileName = null, CancellationToken cancellationToken = default);
}

public class SightengineImageModerationService : IImageModerationService
{
    private const string CheckUrl = "https://api.sightengine.com/1.0/check.json";
    private const string Models = "nudity,violence,gore";
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<SightengineImageModerationService>? _logger;

    private static readonly double NudityRawUmbral = 0.6;
    private static readonly double ViolenceUmbral = 0.5;
    private static readonly double GoreUmbral = 0.5;

    public SightengineImageModerationService(HttpClient httpClient, IConfiguration config, ILogger<SightengineImageModerationService>? logger = null)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public bool IsEnabled =>
        !string.IsNullOrWhiteSpace(_config["Sightengine:ApiUser"]) &&
        !string.IsNullOrWhiteSpace(_config["Sightengine:ApiSecret"]);

    public async Task<(bool Aprobado, string? MensajeRechazo)> ModerateImageAsync(byte[] imageBytes, string? fileName = null, CancellationToken cancellationToken = default)
    {
        if (!IsEnabled)
            return (true, null);
        if (imageBytes == null || imageBytes.Length == 0)
            return (true, null);

        var apiUser = _config["Sightengine:ApiUser"]?.Trim() ?? "";
        var apiSecret = _config["Sightengine:ApiSecret"]?.Trim() ?? "";

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(apiUser), "api_user");
        content.Add(new StringContent(apiSecret), "api_secret");
        content.Add(new StringContent(Models), "models");

        var extension = "jpg";
        if (!string.IsNullOrEmpty(fileName))
        {
            var ext = Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();
            if (ext is "png" or "gif" or "webp") extension = ext;
        }
        var mediaType = extension == "png" ? "image/png" : extension == "gif" ? "image/gif" : "image/jpeg";
        var imageContent = new ByteArrayContent(imageBytes);
        imageContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
        content.Add(imageContent, "media", fileName ?? ("image." + extension));

        using var response = await _httpClient.PostAsync(CheckUrl, content, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger?.LogWarning("Sightengine API error: {Status} {Body}", response.StatusCode, json);
            return (false, "No se pudo verificar la imagen. Intentá de nuevo más tarde.");
        }

        return EvaluarRespuesta(json);
    }

    private (bool Aprobado, string? MensajeRechazo) EvaluarRespuesta(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("nudity", out var nudity))
            {
                if (nudity.TryGetProperty("raw", out var raw) && raw.TryGetDouble(out var rawVal) && rawVal >= NudityRawUmbral)
                    return (false, "El contenido no cumple las normas de la comunidad (contenido inapropiado). No se permiten imágenes con desnudos o contenido sexual.");
            }

            if (root.TryGetProperty("violence", out var violence))
            {
                if (violence.TryGetProperty("prob", out var prob) && prob.TryGetDouble(out var probVal) && probVal >= ViolenceUmbral)
                    return (false, "El contenido no cumple las normas de la comunidad (violencia). No se permiten imágenes violentas.");
            }

            if (root.TryGetProperty("gore", out var gore))
            {
                if (gore.TryGetProperty("prob", out var goreProb) && goreProb.TryGetDouble(out var goreVal) && goreVal >= GoreUmbral)
                    return (false, "El contenido no cumple las normas de la comunidad (contenido gráfico). No se permiten imágenes con contenido sangriento o perturbador.");
            }

            return (true, null);
        }
        catch (JsonException ex)
        {
            _logger?.LogError(ex, "Sightengine: respuesta JSON inválida.");
            return (false, "No se pudo verificar la imagen. Intentá de nuevo más tarde.");
        }
    }
}
