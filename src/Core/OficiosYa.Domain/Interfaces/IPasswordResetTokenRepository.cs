using OficiosYa.Domain.Entities;

namespace OficiosYa.Domain.Interfaces
{
    public interface IPasswordResetTokenRepository : IRepository<PasswordResetToken>
    {
        Task<PasswordResetToken?> GetVigentePorHashAsync(string tokenHash);
        Task InvalidarTokensActivosAsync(int usuarioId);
    }
}
