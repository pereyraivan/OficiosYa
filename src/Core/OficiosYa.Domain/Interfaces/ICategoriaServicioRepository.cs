using OficiosYa.Domain.Entities;

namespace OficiosYa.Domain.Interfaces
{
    public interface ICategoriaServicioRepository : IRepository<CategoriaServicio>
    {
        Task<IEnumerable<CategoriaServicio>> GetByTipoAsync(string tipo, bool soloActivos = true);
        Task<IEnumerable<CategoriaServicio>> GetActivosAsync();
        Task<bool> NombreExistsAsync(string nombre, string tipo, int? excludeId = null);
    }
}

