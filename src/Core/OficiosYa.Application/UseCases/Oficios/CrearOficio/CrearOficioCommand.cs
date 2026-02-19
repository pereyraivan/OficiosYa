using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.Oficios.CrearOficio
{
    public class CrearOficioCommand : IRequest<OficioResponse?>
    {
        public CrearOficioRequest Request { get; set; } = null!;
    }
}

