namespace OficiosYa.Web.Services;

/// <summary>
/// Límites por plan: Free (2 contactos/mes, 1 zona), Básica (15 contactos/mes, zonas ilimitadas), Premium (ilimitado contactos y zonas, con empleos).
/// Las fechas de vencimiento se controlan con Suscripcion.FechaVencimiento y Estado "Activa"; si venció, se considera plan Free.
/// </summary>
public static class PlanLimitsHelper
{
    public const int FreeContactosMensuales = 2;
    public const int BasicaContactosMensuales = 15;
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
            "basica" => BasicaContactosMensuales,
            "premium" => PremiumContactosMensuales,
            "pro" => PremiumContactosMensuales, // legacy
            _ => FreeContactosMensuales
        };
    }

    /// <summary>Indica si el plan permite postularse a empleos (solo Premium).</summary>
    public static bool TieneAccesoEmpleos(string? nombrePlan)
    {
        if (string.IsNullOrWhiteSpace(nombrePlan)) return false;
        var n = nombrePlan.Trim().ToLowerInvariant();
        return n == "premium" || n == "pro";
    }

    /// <summary>Máximo de zonas de cobertura permitidas. Free = 1, Básica y Premium = ilimitadas.</summary>
    public static int GetMaxZonasCobertura(string? nombrePlan)
    {
        if (string.IsNullOrWhiteSpace(nombrePlan)) return FreeZonasCobertura;
        var n = nombrePlan.Trim().ToLowerInvariant();
        return n switch
        {
            "basica" => ZonasCoberturaIlimitadas,
            "premium" => ZonasCoberturaIlimitadas,
            "pro" => ZonasCoberturaIlimitadas,
            _ => FreeZonasCobertura
        };
    }
}
