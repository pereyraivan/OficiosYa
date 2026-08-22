namespace OficiosYa.Web.Options;

public sealed class CuentaTransferenciaConfigItem
{
    public string Etiqueta { get; set; } = string.Empty;

    /// <summary>CVU o alias para transferir (MISMO u otro).</summary>
    public string CvuAlias { get; set; } = string.Empty;

    public string Titular { get; set; } = string.Empty;
}

public sealed class TransferenciaSuscripcionOptions
{
    public List<CuentaTransferenciaConfigItem> Cuentas { get; set; } = new();

    public string MensajeConcepto { get; set; } =
        "En el concepto o detalle de la transferencia escribí tu DNI o CUIT exactamente como figura arriba. Así identificamos tu pago.";
}
