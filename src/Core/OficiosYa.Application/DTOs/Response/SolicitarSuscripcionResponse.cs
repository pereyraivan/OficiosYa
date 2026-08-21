namespace OficiosYa.Application.DTOs.Response
{
    public class SolicitarSuscripcionResponse
    {
        public int SuscripcionId { get; set; }

        public string CodigoReferenciaTransferencia { get; set; } = string.Empty;

        public decimal MontoSuscripcionPendienteArs { get; set; }

        public string PlanNombre { get; set; } = string.Empty;

        public int Meses { get; set; }

        /// <summary>True si ya había una suscripción pendiente y se devolvieron esos datos.</summary>
        public bool YaExistente { get; set; }
    }
}
