using MediatR;
using OficiosYa.Application.UseCases.Usuarios.OlvideContrasena;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Usuarios.RestablecerContrasena
{
    public class RestablecerContrasenaHandler : IRequestHandler<RestablecerContrasenaCommand, RestablecerContrasenaResult>
    {
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public RestablecerContrasenaHandler(
            IPasswordResetTokenRepository tokenRepository,
            IUsuarioRepository usuarioRepository)
        {
            _tokenRepository = tokenRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<RestablecerContrasenaResult> Handle(RestablecerContrasenaCommand request, CancellationToken cancellationToken)
        {
            var tokenPlano = request.Request.Token.Trim();
            if (string.IsNullOrEmpty(tokenPlano))
            {
                return Error("El enlace de recuperación no es válido o ya expiró.");
            }

            var tokenHash = OlvideContrasenaHandler.HashToken(tokenPlano);
            var resetToken = await _tokenRepository.GetVigentePorHashAsync(tokenHash);

            if (resetToken?.Usuario == null || !resetToken.Usuario.Activo)
            {
                return Error("El enlace de recuperación no es válido o ya expiró.");
            }

            resetToken.Usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.NuevaContrasena);
            resetToken.MarcarComoUsado();

            await _usuarioRepository.UpdateAsync(resetToken.Usuario);
            await _tokenRepository.UpdateAsync(resetToken);
            await _tokenRepository.InvalidarTokensActivosAsync(resetToken.UsuarioId);

            return new RestablecerContrasenaResult
            {
                Success = true,
                Message = "Tu contraseña fue actualizada. Ya podés iniciar sesión."
            };
        }

        private static RestablecerContrasenaResult Error(string message) =>
            new() { Success = false, Message = message };
    }
}
