using OficiosYa.Domain.Entities;

namespace OficiosYa.Domain.Interfaces
{
    public interface ITipoServicioRepository : IRepository<TipoServicio>
    {
        Task<TipoServicio?> GetByNombreAsync(string nombre);
        Task<List<TipoServicio>> GetAllActivosAsync();
    }
}

