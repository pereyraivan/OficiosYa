using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.CategoriaServicios.ActualizarCategoriaServicio
{
    public class ActualizarCategoriaServicioHandler : IRequestHandler<ActualizarCategoriaServicioCommand, CategoriaServicioResponse?>
    {
        private readonly ICategoriaServicioRepository _categoriaRepository;
        private readonly ITipoServicioRepository _tipoServicioRepository;

        public ActualizarCategoriaServicioHandler(
            ICategoriaServicioRepository categoriaRepository, 
            ITipoServicioRepository tipoServicioRepository)
        {
            _categoriaRepository = categoriaRepository;
            _tipoServicioRepository = tipoServicioRepository;
        }

        public async Task<CategoriaServicioResponse?> Handle(ActualizarCategoriaServicioCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(request.Request.Id);
            if (categoria == null)
            {
                return null; // No encontrado
            }

            // Validar tipo y buscar TipoServicio
            var tipoServicio = await _tipoServicioRepository.GetByNombreAsync(request.Request.Tipo);

            if (tipoServicio == null)
            {
                return null; // Tipo inválido
            }

            // Verificar si el nombre ya existe en otra categoría del mismo tipo
            if (await _categoriaRepository.NombreExistsAsync(request.Request.Nombre, request.Request.Tipo, request.Request.Id))
            {
                return null; // Nombre duplicado
            }

            categoria.Nombre = request.Request.Nombre;
            categoria.Descripcion = request.Request.Descripcion;
            categoria.TipoServicioId = tipoServicio.Id;
            categoria.Icono = request.Request.Icono;
            categoria.Activo = request.Request.Activo;

            await _categoriaRepository.UpdateAsync(categoria);
            
            // Cargar TipoServicio para el response
            tipoServicio = await _tipoServicioRepository.GetByIdAsync(tipoServicio.Id);

            return new CategoriaServicioResponse
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Tipo = tipoServicio?.Nombre ?? string.Empty,
                Icono = categoria.Icono,
                Activo = categoria.Activo,
                FechaCreacion = categoria.FechaCreacion
            };
        }
    }
}

