using Microsoft.EntityFrameworkCore;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Persistence.Context;

namespace OficiosYa.Persistence.Repositories
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

        public async Task<bool> DocumentoExistsAsync(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return false;

            return await _dbSet.AnyAsync(u => u.Documento == documento);
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

        public async Task<bool> VerificarEmailAsync(string email, string token)
        {
            var usuario = await GetByEmailAsync(email);
            if (usuario == null) return false;

            // Aquí implementarías la lógica de verificación de email
            // Por ahora, simplemente marcamos como verificado
            usuario.VerificarEmail();
            await UpdateAsync(usuario);
            return true;
        }
    }
}
