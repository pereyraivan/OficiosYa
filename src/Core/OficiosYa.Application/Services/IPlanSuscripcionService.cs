using OficiosYa.Domain.Entities;

namespace OficiosYa.Application.Services
{
    public interface IPlanSuscripcionService
    {
        Task<List<PlanSuscripcion>> ObtenerPlanesActivosAsync();
        Task<PlanSuscripcion?> ObtenerPlanPorNombreAsync(string nombre);
        Task<decimal> CalcularPrecioAsync(string nombrePlan, int meses);
        Task<decimal> CalcularPrecioConDescuentoAsync(string nombrePlan, int meses);
    }
}

