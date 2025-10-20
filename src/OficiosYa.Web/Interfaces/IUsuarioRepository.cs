using OficiosYa.Web.Models;

namespace OficiosYa.Web.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<Usuario>> GetActiveUsersAsync();
        Task<Usuario?> GetWithProfesionalesAsync(int id);
    }
}
