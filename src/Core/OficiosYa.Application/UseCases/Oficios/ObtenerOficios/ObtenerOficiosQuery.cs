using MediatR;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.Oficios.ObtenerOficios
{
    public class ObtenerOficiosQuery : IRequest<IEnumerable<OficioResponse>>
    {
        public bool SoloActivos { get; set; } = true;
    }
}

