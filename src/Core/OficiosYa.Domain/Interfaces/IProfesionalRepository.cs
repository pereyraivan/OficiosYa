using OficiosYa.Domain.Entities;

namespace OficiosYa.Domain.Interfaces
{
    public interface IProfesionalRepository : IRepository<Profesional>
    {
        Task<IEnumerable<Profesional>> GetByCategoriaAsync(int categoriaId);
        Task<IEnumerable<Profesional>> GetDisponiblesAsync();
        Task<IEnumerable<Profesional>> BuscarAsync(string? termino, int? categoriaId, string? localidad, string? provincia);
        Task<Profesional?> GetWithCategoriasAsync(int id);
        Task<Profesional?> GetWithResenasAsync(int id);
        Task<Profesional?> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Profesional>> GetTopRatedAsync(int cantidad = 10);
        Task<IEnumerable<Profesional>> GetNearbyAsync(double latitud, double longitud, double radioKm = 10);
        Task AsociarCategoriasAsync(int profesionalId, IEnumerable<string> nombresCategorias);
        Task AsociarZonasCoberturaAsync(int profesionalId, IEnumerable<ZonaCobertura> zonas);
        Task<IEnumerable<Profesional>> BuscarPorLocalidadAsync(string? localidad, string? provincia);
    }
}
