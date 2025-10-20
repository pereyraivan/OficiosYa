using OficiosYa.Web.Models.DTOs;

namespace OficiosYa.Web.Interfaces
{
    public interface IProfesionalService
    {
        Task<ProfesionalResponse?> RegistrarProfesionalAsync(RegistrarProfesionalRequest request);
        Task<ProfesionalResponse?> ObtenerProfesionalPorIdAsync(int id);
        Task<ProfesionalResponse?> ObtenerProfesionalPorUsuarioIdAsync(int usuarioId);
        Task<List<ProfesionalResponse>> ObtenerProfesionalesDestacadosAsync();
        Task<List<ProfesionalResponse>> BuscarProfesionalesAsync(string oficio, string zona, int radio = 10);
        Task<List<ProfesionalResponse>> ObtenerProfesionalesPorOficioAsync(string oficio);
        Task<List<ProfesionalResponse>> ObtenerProfesionalesCercaDeAsync(string ciudad, int radio = 10);
        Task<bool> ActualizarDisponibilidadAsync(int id, bool disponible);
        Task<bool> ActualizarPerfilAsync(int id, string? descripcion, string? fotoPerfil);
    }
}
