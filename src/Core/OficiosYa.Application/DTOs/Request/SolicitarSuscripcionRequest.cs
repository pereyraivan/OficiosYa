namespace OficiosYa.Application.DTOs.Request
{
    public class SolicitarSuscripcionRequest
    {
        public string PlanSeleccionado { get; set; } = string.Empty;

        public int MesesSuscripcion { get; set; } = 1;

        public string? MetodoPago { get; set; }
    }
}
