using MediatR;
using OficiosYa.Application.DTOs.Request;

namespace OficiosYa.Application.UseCases.Usuarios.RestablecerContrasena
{
    public class RestablecerContrasenaCommand : IRequest<RestablecerContrasenaResult>
    {
        public RestablecerContrasenaRequest Request { get; set; } = null!;
    }

    public class RestablecerContrasenaResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
