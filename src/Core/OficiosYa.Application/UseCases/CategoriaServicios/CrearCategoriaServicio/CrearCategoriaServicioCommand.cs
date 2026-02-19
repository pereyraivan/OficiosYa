using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.CategoriaServicios.CrearCategoriaServicio
{
    public class CrearCategoriaServicioCommand : IRequest<CategoriaServicioResponse?>
    {
        public CrearCategoriaServicioRequest Request { get; set; } = null!;
    }
}

