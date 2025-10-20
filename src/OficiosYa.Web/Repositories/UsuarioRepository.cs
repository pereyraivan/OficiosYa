using Microsoft.EntityFrameworkCore;
using OficiosYa.Web.Data;
using OficiosYa.Web.Interfaces;
using OficiosYa.Web.Models;

namespace OficiosYa.Web.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<Usuario>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(u => u.Activo)
                .ToListAsync();
        }

        public async Task<Usuario?> GetWithProfesionalesAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Profesionales)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
