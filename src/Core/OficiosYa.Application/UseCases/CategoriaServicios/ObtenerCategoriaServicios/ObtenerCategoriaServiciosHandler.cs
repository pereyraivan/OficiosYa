using MediatR;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.CategoriaServicios.ObtenerCategoriaServicios
{
    public class ObtenerCategoriaServiciosHandler : IRequestHandler<ObtenerCategoriaServiciosQuery, IEnumerable<CategoriaServicioResponse>>
    {
        private readonly ICategoriaServicioRepository _categoriaRepository;

        public ObtenerCategoriaServiciosHandler(ICategoriaServicioRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaServicioResponse>> Handle(ObtenerCategoriaServiciosQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.CategoriaServicio> categorias;

            if (!string.IsNullOrEmpty(request.Tipo))
            {
                categorias = await _categoriaRepository.GetByTipoAsync(request.Tipo);
            }
            else if (request.SoloActivos)
            {
                categorias = await _categoriaRepository.GetActivosAsync();
            }
            else
            {
                categorias = await _categoriaRepository.GetAllAsync();
            }

            return categorias.Select(c => new CategoriaServicioResponse
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Tipo = c.TipoServicio?.Nombre ?? string.Empty,
                Icono = c.Icono,
                Activo = c.Activo,
                FechaCreacion = c.FechaCreacion
            });
        }
    }
}

