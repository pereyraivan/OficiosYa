using MediatR;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Oficios.ObtenerOficios
{
    public class ObtenerOficiosHandler : IRequestHandler<ObtenerOficiosQuery, IEnumerable<OficioResponse>>
    {
        private readonly IOficioRepository _oficioRepository;

        public ObtenerOficiosHandler(IOficioRepository oficioRepository)
        {
            _oficioRepository = oficioRepository;
        }

        public async Task<IEnumerable<OficioResponse>> Handle(ObtenerOficiosQuery request, CancellationToken cancellationToken)
        {
            var oficios = request.SoloActivos 
                ? await _oficioRepository.GetActivosAsync()
                : await _oficioRepository.GetAllAsync();

            return oficios.Select(o => new OficioResponse
            {
                Id = o.Id,
                Nombre = o.Nombre,
                Descripcion = o.Descripcion,
                Icono = o.Icono,
                Color = o.Color,
                Activo = o.Activo,
                FechaCreacion = o.FechaCreacion
            });
        }
    }
}

