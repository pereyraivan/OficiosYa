namespace OficiosYa.Web.Services;

/// <summary>
/// Límites por plan: Free (5 contactos/mes), Básico (20), Pro (ilimitado), Premium (ilimitado).
/// Las fechas de vencimiento se controlan con Suscripcion.FechaVencimiento y Estado "Activa"; si venció, se considera plan Free.
/// </summary>
public static class PlanLimitsHelper
{
    public const int FreeContactosMensuales = 5;
    public const int BasicaContactosMensuales = 20;
    /// <summary>Valor alto para "ilimitado"; no se debe alcanzar en la práctica.</summary>
    public const int PremiumContactosMensuales = 99999;

    public const int FreeZonasCobertura = 1;
    /// <summary>Valor alto para zonas ilimitadas en Básica y Premium.</summary>
    public const int ZonasCoberturaIlimitadas = 99999;

    /// <summary>Obtiene el límite de contactos por mes para el plan. "Free" si el nombre no es reconocido.</summary>
    public static int GetLimiteContactosMensuales(string? nombrePlan)
    {
        if (string.IsNullOrWhiteSpace(nombrePlan)) return FreeContactosMensuales;
        var n = nombrePlan.Trim().ToLowerInvariant();
        return n switch
        {
            "basico" => BasicaContactosMensuales,
            "basica" => BasicaContactosMensuales,
            "pro" => PremiumContactosMensuales,
            "premium" => PremiumContactosMensuales,
            _ => FreeContactosMensuales
        };
    }

    /// <summary>Indica si el plan permite postularse a empleos (Pro y Premium).</summary>
    public static bool TieneAccesoEmpleos(string? nombrePlan)
    {
        if (string.IsNullOrWhiteSpace(nombrePlan)) return false;
        var n = nombrePlan.Trim().ToLowerInvariant();
        return n == "premium" || n == "pro";
    }

    /// <summary>Máximo de zonas de cobertura permitidas. Free = 1, resto = ilimitadas.</summary>
    public static int GetMaxZonasCobertura(string? nombrePlan)
    {
        if (string.IsNullOrWhiteSpace(nombrePlan)) return FreeZonasCobertura;
        var n = nombrePlan.Trim().ToLowerInvariant();
        return n switch
        {
            "basico" => ZonasCoberturaIlimitadas,
            "basica" => ZonasCoberturaIlimitadas,
            "pro" => ZonasCoberturaIlimitadas,
            "premium" => ZonasCoberturaIlimitadas,
            _ => FreeZonasCobertura
        };
    }
}
