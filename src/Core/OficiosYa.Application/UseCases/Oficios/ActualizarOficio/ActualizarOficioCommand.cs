using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.Oficios.ActualizarOficio
{
    public class ActualizarOficioCommand : IRequest<OficioResponse?>
    {
        public ActualizarOficioRequest Request { get; set; } = null!;
    }
}

