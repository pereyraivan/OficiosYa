using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Web.Contracts;

public sealed class RegistrarProfesionalApiResponse
{
    public ProfesionalResponse Profesional { get; set; } = null!;

    /// <summary>Datos para transferir (CVU/alias) y código de concepto; solo si el plan es de pago por transferencia.</summary>
    public InstruccionesPagoTransferenciaDto? InstruccionesTransferencia { get; set; }
}
