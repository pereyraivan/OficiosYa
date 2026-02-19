using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Oficios.ActualizarOficio
{
    public class ActualizarOficioHandler : IRequestHandler<ActualizarOficioCommand, OficioResponse?>
    {
        private readonly IOficioRepository _oficioRepository;

        public ActualizarOficioHandler(IOficioRepository oficioRepository)
        {
            _oficioRepository = oficioRepository;
        }

        public async Task<OficioResponse?> Handle(ActualizarOficioCommand request, CancellationToken cancellationToken)
        {
            var oficio = await _oficioRepository.GetByIdAsync(request.Request.Id);
            if (oficio == null)
            {
                return null; // No encontrado
            }

            // Verificar si el nombre ya existe en otro oficio
            if (await _oficioRepository.NombreExistsAsync(request.Request.Nombre, request.Request.Id))
            {
                return null; // Nombre duplicado
            }

            oficio.Nombre = request.Request.Nombre;
            oficio.Descripcion = request.Request.Descripcion;
            oficio.Icono = request.Request.Icono;
            oficio.Color = request.Request.Color;
            oficio.Activo = request.Request.Activo;

            await _oficioRepository.UpdateAsync(oficio);

            return new OficioResponse
            {
                Id = oficio.Id,
                Nombre = oficio.Nombre,
                Descripcion = oficio.Descripcion,
                Icono = oficio.Icono,
                Color = oficio.Color,
                Activo = oficio.Activo,
                FechaCreacion = oficio.FechaCreacion
            };
        }
    }
}

