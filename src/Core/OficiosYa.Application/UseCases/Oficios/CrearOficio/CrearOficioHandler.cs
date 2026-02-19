using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Oficios.CrearOficio
{
    public class CrearOficioHandler : IRequestHandler<CrearOficioCommand, OficioResponse?>
    {
        private readonly IOficioRepository _oficioRepository;

        public CrearOficioHandler(IOficioRepository oficioRepository)
        {
            _oficioRepository = oficioRepository;
        }

        public async Task<OficioResponse?> Handle(CrearOficioCommand request, CancellationToken cancellationToken)
        {
            // Verificar si el nombre ya existe
            if (await _oficioRepository.NombreExistsAsync(request.Request.Nombre))
            {
                return null; // Nombre duplicado
            }

            var oficio = new Oficio
            {
                Nombre = request.Request.Nombre,
                Descripcion = request.Request.Descripcion,
                Icono = request.Request.Icono,
                Color = request.Request.Color,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            var oficioCreado = await _oficioRepository.AddAsync(oficio);

            return new OficioResponse
            {
                Id = oficioCreado.Id,
                Nombre = oficioCreado.Nombre,
                Descripcion = oficioCreado.Descripcion,
                Icono = oficioCreado.Icono,
                Color = oficioCreado.Color,
                Activo = oficioCreado.Activo,
                FechaCreacion = oficioCreado.FechaCreacion
            };
        }
    }
}

