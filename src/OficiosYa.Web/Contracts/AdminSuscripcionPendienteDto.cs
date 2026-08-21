namespace OficiosYa.Web.Contracts;

public sealed class AdminSuscripcionPendienteDto
{
    public int Id { get; set; }
    public int ProfesionalId { get; set; }
    public string ProfesionalNombre { get; set; } = string.Empty;
    public string ProfesionalEmail { get; set; } = string.Empty;
    public string? ProfesionalTelefono { get; set; }
    public int? PlanSuscripcionId { get; set; }
    public string PlanNombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string? CodigoPago { get; set; }
    public string? MetodoPago { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int MesesDuracion { get; set; }
}

public sealed class AdminConfirmarSuscripcionResponse
{
    public int Id { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool PagoConfirmado { get; set; }
    public DateTime? FechaPago { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Mensaje { get; set; } = string.Empty;
}
