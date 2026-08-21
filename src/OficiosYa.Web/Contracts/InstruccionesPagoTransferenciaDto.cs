namespace OficiosYa.Web.Contracts;

public sealed class InstruccionesTransferenciaCuentaDto
{
    public string Etiqueta { get; set; } = string.Empty;

    public string CvuAlias { get; set; } = string.Empty;

    public string Titular { get; set; } = string.Empty;
}

public sealed class InstruccionesPagoTransferenciaDto
{
    public string CodigoReferencia { get; set; } = string.Empty;

    public decimal MontoArs { get; set; }

    public string Moneda { get; set; } = "ARS";

    public string PlanNombre { get; set; } = string.Empty;

    public int Meses { get; set; }

    public string MensajeConcepto { get; set; } = string.Empty;

    public List<InstruccionesTransferenciaCuentaDto> Cuentas { get; set; } = new();
}
