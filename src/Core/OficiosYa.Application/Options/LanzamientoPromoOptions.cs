namespace OficiosYa.Application.Options
{
    /// <summary>
    /// Promo de lanzamiento: al registrarse, el profesional recibe un plan gratis por N días.
    /// </summary>
    public class LanzamientoPromoOptions
    {
        public const string SectionName = "LanzamientoPromo";

        /// <summary>Si es true y no venció FechaFinUtc, los nuevos registros reciben el plan gratis.</summary>
        public bool Enabled { get; set; }

        /// <summary>Nombre del plan en BD (ej. Premium).</summary>
        public string PlanNombre { get; set; } = "Premium";

        /// <summary>Días de plan gratis desde el registro.</summary>
        public int DiasGratis { get; set; } = 30;

        /// <summary>
        /// Fin de la campaña (UTC). Después de esta fecha, Enabled se ignora y no se aplica promo a nuevos.
        /// Null = mientras Enabled sea true.
        /// </summary>
        public DateTime? FechaFinUtc { get; set; }

        /// <summary>Texto para mostrar en registro / planes.</summary>
        public string MensajePublico { get; set; } =
            "¡Lanzamiento! Registrate y usá Premium gratis durante 30 días.";

        public bool EstaActivaAhora()
        {
            if (!Enabled) return false;
            if (FechaFinUtc.HasValue && DateTime.UtcNow > FechaFinUtc.Value) return false;
            if (DiasGratis <= 0) return false;
            return !string.IsNullOrWhiteSpace(PlanNombre);
        }
    }
}
