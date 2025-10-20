using OficiosYa.Web.Models;

namespace OficiosYa.Web.Interfaces
{
    public interface IProfesionalRepository : IRepository<Profesional>
    {
        Task<Profesional?> ObtenerPorUsuarioIdAsync(int usuarioId);
        Task<List<Profesional>> ObtenerDestacadosAsync();
        Task<List<Profesional>> BuscarPorOficioAsync(string oficio);
        Task<List<Profesional>> BuscarCercaDeAsync(string ciudad, int radio = 10);
        Task<List<Profesional>> ObtenerConOficiosAsync();
    }
}
