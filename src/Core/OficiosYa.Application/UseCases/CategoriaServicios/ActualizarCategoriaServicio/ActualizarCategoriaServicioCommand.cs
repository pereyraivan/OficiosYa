using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.CategoriaServicios.ActualizarCategoriaServicio
{
    public class ActualizarCategoriaServicioCommand : IRequest<CategoriaServicioResponse?>
    {
        public ActualizarCategoriaServicioRequest Request { get; set; } = null!;
    }
}

