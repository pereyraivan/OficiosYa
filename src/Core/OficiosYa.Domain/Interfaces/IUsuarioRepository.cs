using OficiosYa.Domain.Entities;

namespace OficiosYa.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> DocumentoExistsAsync(string documento);
        Task<IEnumerable<Usuario>> GetActiveUsersAsync();
        Task<Usuario?> GetWithProfesionalesAsync(int id);
        Task<bool> VerificarEmailAsync(string email, string token);
    }
}
