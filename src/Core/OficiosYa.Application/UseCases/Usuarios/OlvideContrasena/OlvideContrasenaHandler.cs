using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using OficiosYa.Application.Services;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Usuarios.OlvideContrasena
{
    public class OlvideContrasenaHandler : IRequestHandler<OlvideContrasenaCommand, OlvideContrasenaResult>
    {
        private const int TokenExpiracionHoras = 1;
        private const string MensajeGenerico =
            "Si el email está registrado, recibirás un enlace para restablecer tu contraseña en los próximos minutos.";

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IPasswordResetEmailService _emailService;
        private readonly ILogger<OlvideContrasenaHandler> _logger;

        public OlvideContrasenaHandler(
            IUsuarioRepository usuarioRepository,
            IPasswordResetTokenRepository tokenRepository,
            IPasswordResetEmailService emailService,
            ILogger<OlvideContrasenaHandler> logger)
        {
            _usuarioRepository = usuarioRepository;
            _tokenRepository = tokenRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<OlvideContrasenaResult> Handle(OlvideContrasenaCommand request, CancellationToken cancellationToken)
        {
            var email = request.Request.Email.Trim();
            var usuario = await _usuarioRepository.GetByEmailAsync(email);

            if (usuario == null || !usuario.Activo)
            {
                _logger.LogInformation(
                    "Recuperación solicitada para {Email}: no se envió mail (usuario inexistente o inactivo).",
                    email);
            }
            else
            {
                var tokenPlano = GenerarTokenSeguro();
                var tokenHash = HashToken(tokenPlano);

                await _tokenRepository.InvalidarTokensActivosAsync(usuario.Id);

                await _tokenRepository.AddAsync(new PasswordResetToken
                {
                    UsuarioId = usuario.Id,
                    TokenHash = tokenHash,
                    ExpiresAt = DateTime.UtcNow.AddHours(TokenExpiracionHoras),
                    CreatedAt = DateTime.UtcNow
                });

                _logger.LogInformation("Recuperación para {Email}: generando token y enviando mail...", email);
                await _emailService.EnviarEnlaceRecuperacionAsync(usuario, tokenPlano, cancellationToken);
            }

            return new OlvideContrasenaResult
            {
                Success = true,
                Message = MensajeGenerico
            };
        }

        private static string GenerarTokenSeguro()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-", StringComparison.Ordinal)
                .Replace("/", "_", StringComparison.Ordinal)
                .TrimEnd('=');
        }

        internal static string HashToken(string token)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(hash);
        }
    }
}
