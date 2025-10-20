using OficiosYa.Web.Models.DTOs;

namespace OficiosYa.Web.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse?> RegistrarUsuarioAsync(RegistrarUsuarioRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<UsuarioResponse?> ObtenerUsuarioPorIdAsync(int id);
        Task<bool> VerificarEmailDisponibleAsync(string email);
        Task<bool> VerificarEmailAsync(string email, string token);
    }
}
