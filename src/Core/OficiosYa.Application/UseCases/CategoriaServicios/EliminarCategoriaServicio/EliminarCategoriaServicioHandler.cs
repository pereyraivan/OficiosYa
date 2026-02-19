using MediatR;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.CategoriaServicios.EliminarCategoriaServicio
{
    public class EliminarCategoriaServicioHandler : IRequestHandler<EliminarCategoriaServicioCommand, bool>
    {
        private readonly ICategoriaServicioRepository _categoriaRepository;

        public EliminarCategoriaServicioHandler(ICategoriaServicioRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<bool> Handle(EliminarCategoriaServicioCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(request.Id);
            if (categoria == null)
            {
                return false;
            }

            // En lugar de eliminar físicamente, desactivamos
            categoria.Desactivar();
            await _categoriaRepository.UpdateAsync(categoria);

            return true;
        }
    }
}

