using OficiosYa.Domain.Entities;

namespace OficiosYa.Domain.Interfaces
{
    public interface IPlanSuscripcionRepository : IRepository<PlanSuscripcion>
    {
        Task<List<PlanSuscripcion>> GetAllActivosAsync();
        Task<PlanSuscripcion?> GetByNombreAsync(string nombre);
    }
}

