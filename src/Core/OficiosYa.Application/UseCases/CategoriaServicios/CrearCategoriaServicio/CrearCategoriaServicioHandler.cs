using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.CategoriaServicios.CrearCategoriaServicio
{
    public class CrearCategoriaServicioHandler : IRequestHandler<CrearCategoriaServicioCommand, CategoriaServicioResponse?>
    {
        private readonly ICategoriaServicioRepository _categoriaRepository;
        private readonly ITipoServicioRepository _tipoServicioRepository;

        public CrearCategoriaServicioHandler(
            ICategoriaServicioRepository categoriaRepository, 
            ITipoServicioRepository tipoServicioRepository)
        {
            _categoriaRepository = categoriaRepository;
            _tipoServicioRepository = tipoServicioRepository;
        }

        public async Task<CategoriaServicioResponse?> Handle(CrearCategoriaServicioCommand request, CancellationToken cancellationToken)
        {
            // Validar tipo y buscar TipoServicio
            var tipoServicio = await _tipoServicioRepository.GetByNombreAsync(request.Request.Tipo);

            if (tipoServicio == null)
            {
                return null; // Tipo inválido
            }

            // Verificar si el nombre ya existe para este tipo
            if (await _categoriaRepository.NombreExistsAsync(request.Request.Nombre, request.Request.Tipo))
            {
                return null; // Nombre duplicado
            }

            var categoria = new CategoriaServicio
            {
                Nombre = request.Request.Nombre,
                Descripcion = request.Request.Descripcion,
                TipoServicioId = tipoServicio.Id,
                Icono = request.Request.Icono,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            var categoriaCreada = await _categoriaRepository.AddAsync(categoria);
            
            // Cargar TipoServicio para el response
            tipoServicio = await _tipoServicioRepository.GetByIdAsync(tipoServicio.Id);

            return new CategoriaServicioResponse
            {
                Id = categoriaCreada.Id,
                Nombre = categoriaCreada.Nombre,
                Descripcion = categoriaCreada.Descripcion,
                Tipo = tipoServicio?.Nombre ?? string.Empty,
                Icono = categoriaCreada.Icono,
                Activo = categoriaCreada.Activo,
                FechaCreacion = categoriaCreada.FechaCreacion
            };
        }
    }
}

