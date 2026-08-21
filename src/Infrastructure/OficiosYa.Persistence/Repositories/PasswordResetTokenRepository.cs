using Microsoft.EntityFrameworkCore;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Persistence.Context;

namespace OficiosYa.Persistence.Repositories
{
    public class PasswordResetTokenRepository : Repository<PasswordResetToken>, IPasswordResetTokenRepository
    {
        public PasswordResetTokenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PasswordResetToken?> GetVigentePorHashAsync(string tokenHash)
        {
            var ahora = DateTime.UtcNow;
            return await _dbSet
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t =>
                    t.TokenHash == tokenHash &&
                    t.UsedAt == null &&
                    t.ExpiresAt > ahora);
        }

        public async Task InvalidarTokensActivosAsync(int usuarioId)
        {
            var ahora = DateTime.UtcNow;
            var tokens = await _dbSet
                .Where(t => t.UsuarioId == usuarioId && t.UsedAt == null && t.ExpiresAt > ahora)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.MarcarComoUsado();
            }

            if (tokens.Count > 0)
            {
                await UpdateRangeAsync(tokens);
            }
        }
    }
}
