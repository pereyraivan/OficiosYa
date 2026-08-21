using MediatR;
using OficiosYa.Application.DTOs.Request;

namespace OficiosYa.Application.UseCases.Usuarios.OlvideContrasena
{
    public class OlvideContrasenaCommand : IRequest<OlvideContrasenaResult>
    {
        public OlvideContrasenaRequest Request { get; set; } = null!;
    }

    public class OlvideContrasenaResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
