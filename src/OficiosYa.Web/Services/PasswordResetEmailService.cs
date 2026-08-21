using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OficiosYa.Application.Services;
using OficiosYa.Domain.Entities;
using OficiosYa.Web.Options;

namespace OficiosYa.Web.Services
{
    public class PasswordResetEmailService : IPasswordResetEmailService
    {
        private readonly EmailOptions _emailOptions;
        private readonly AppOptions _appOptions;
        private readonly ILogger<PasswordResetEmailService> _logger;

        public PasswordResetEmailService(IOptions<EmailOptions> emailOptions,IOptions<AppOptions> appOptions,ILogger<PasswordResetEmailService> logger)
        {
            _emailOptions = emailOptions.Value;
            _appOptions = appOptions.Value;
            _logger = logger;
        }

        public async Task EnviarEnlaceRecuperacionAsync(Usuario usuario,string tokenPlano,CancellationToken cancellationToken = default)
        {
            var baseUrl = _appOptions.FrontendBaseUrl.TrimEnd('/');
            var enlace = $"{baseUrl}/restablecer-contrasena?token={Uri.EscapeDataString(tokenPlano)}";
            var nombre = usuario.ObtenerNombreCompleto();
            var asunto = "Restablecer tu contraseña — OficiosYa";
            var cuerpoHtml = $@"
<!DOCTYPE html>
<html>
<body style=""font-family: Arial, sans-serif; color: #1e293b; line-height: 1.5;"">
  <p>Hola {System.Net.WebUtility.HtmlEncode(nombre)},</p>
  <p>Recibimos una solicitud para restablecer la contraseña de tu cuenta en OficiosYa.</p>
  <p>
    <a href=""{enlace}"" style=""display: inline-block; padding: 12px 24px; background: #059669; color: #fff; text-decoration: none; border-radius: 8px; font-weight: 600;"">
      Restablecer contraseña
    </a>
  </p>
  <p style=""color: #64748b; font-size: 14px;"">Este enlace expira en 1 hora. Si no solicitaste este cambio, podés ignorar este correo.</p>
  <p style=""color: #64748b; font-size: 12px; word-break: break-all;"">Si el botón no funciona, copiá este enlace en tu navegador:<br/>{System.Net.WebUtility.HtmlEncode(enlace)}</p>
</body>
</html>";

            if (!_emailOptions.Enabled || string.IsNullOrWhiteSpace(_emailOptions.SmtpHost))
            {
                _logger.LogWarning(
                    "SMTP desactivado (Email:Enabled=false o sin SmtpHost). Enlace para {Email}: {Enlace}",
                    usuario.Email,
                    enlace);
                return;
            }

            try
            {
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(_emailOptions.FromName, _emailOptions.FromAddress));
                mensaje.To.Add(MailboxAddress.Parse(usuario.Email));
                mensaje.Subject = asunto;
                mensaje.Body = new BodyBuilder { HtmlBody = cuerpoHtml }.ToMessageBody();

                // Gmail muestra la app password con espacios; SMTP la espera sin espacios.
                var smtpPassword = (_emailOptions.Password ?? string.Empty).Replace(" ", "");

                using var cliente = new SmtpClient();
                await cliente.ConnectAsync(
                    _emailOptions.SmtpHost,
                    _emailOptions.SmtpPort,
                    _emailOptions.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                    cancellationToken);

                if (!string.IsNullOrWhiteSpace(_emailOptions.Username))
                {
                    await cliente.AuthenticateAsync(_emailOptions.Username, smtpPassword, cancellationToken);
                }

                await cliente.SendAsync(mensaje, cancellationToken);
                await cliente.DisconnectAsync(true, cancellationToken);

                _logger.LogInformation("Email de recuperación ENVIADO OK a {Email}", usuario.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falló el envío SMTP de recuperación a {Email}", usuario.Email);
                throw;
            }
        }
    }
}
