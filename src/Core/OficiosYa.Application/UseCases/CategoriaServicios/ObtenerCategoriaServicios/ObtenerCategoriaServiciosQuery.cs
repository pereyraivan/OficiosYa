using MediatR;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.CategoriaServicios.ObtenerCategoriaServicios
{
    public class ObtenerCategoriaServiciosQuery : IRequest<IEnumerable<CategoriaServicioResponse>>
    {
        public string? Tipo { get; set; }
        public bool SoloActivos { get; set; } = true;
    }
}

