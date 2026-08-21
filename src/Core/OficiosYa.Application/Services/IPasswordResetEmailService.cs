using OficiosYa.Domain.Entities;

namespace OficiosYa.Application.Services
{
    public interface IPasswordResetEmailService
    {
        Task EnviarEnlaceRecuperacionAsync(Usuario usuario, string tokenPlano, CancellationToken cancellationToken = default);
    }
}
