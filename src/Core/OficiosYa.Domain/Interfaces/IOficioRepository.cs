using OficiosYa.Domain.Entities;

namespace OficiosYa.Domain.Interfaces
{
    public interface IOficioRepository : IRepository<Oficio>
    {
        Task<IEnumerable<Oficio>> GetActivosAsync();
        Task<bool> NombreExistsAsync(string nombre, int? excludeId = null);
    }
}

