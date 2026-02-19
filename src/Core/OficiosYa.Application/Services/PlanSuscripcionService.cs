using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.Services
{
    public class PlanSuscripcionService : IPlanSuscripcionService
    {
        private readonly IPlanSuscripcionRepository _repository;

        public PlanSuscripcionService(IPlanSuscripcionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PlanSuscripcion>> ObtenerPlanesActivosAsync()
        {
            return await _repository.GetAllActivosAsync();
        }

        public async Task<PlanSuscripcion?> ObtenerPlanPorNombreAsync(string nombre)
        {
            return await _repository.GetByNombreAsync(nombre);
        }

        public async Task<decimal> CalcularPrecioAsync(string nombrePlan, int meses)
        {
            var plan = await _repository.GetByNombreAsync(nombrePlan);
            if (plan == null)
                throw new ArgumentException($"Plan '{nombrePlan}' no encontrado");

            return plan.CalcularPrecio(meses);
        }

        public async Task<decimal> CalcularPrecioConDescuentoAsync(string nombrePlan, int meses)
        {
            var plan = await _repository.GetByNombreAsync(nombrePlan);
            if (plan == null)
                throw new ArgumentException($"Plan '{nombrePlan}' no encontrado");

            return plan.CalcularPrecioConDescuento(meses);
        }
    }
}

