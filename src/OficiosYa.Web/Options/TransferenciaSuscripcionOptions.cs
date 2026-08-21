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
        "Incluí este código exacto en el concepto o detalle de la transferencia. Así identificamos tu pago al acreditar.";
}
