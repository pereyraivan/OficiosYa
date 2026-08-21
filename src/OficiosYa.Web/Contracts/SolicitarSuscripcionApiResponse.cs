namespace OficiosYa.Web.Contracts;

public sealed class SolicitarSuscripcionApiResponse
{
    public int SuscripcionId { get; set; }

    public bool YaExistente { get; set; }

    public InstruccionesPagoTransferenciaDto? InstruccionesTransferencia { get; set; }
}
