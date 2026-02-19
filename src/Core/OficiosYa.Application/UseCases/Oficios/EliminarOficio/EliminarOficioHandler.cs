using MediatR;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Oficios.EliminarOficio
{
    public class EliminarOficioHandler : IRequestHandler<EliminarOficioCommand, bool>
    {
        private readonly IOficioRepository _oficioRepository;

        public EliminarOficioHandler(IOficioRepository oficioRepository)
        {
            _oficioRepository = oficioRepository;
        }

        public async Task<bool> Handle(EliminarOficioCommand request, CancellationToken cancellationToken)
        {
            var oficio = await _oficioRepository.GetByIdAsync(request.Id);
            if (oficio == null)
            {
                return false;
            }

            // En lugar de eliminar físicamente, desactivamos
            oficio.Desactivar();
            await _oficioRepository.UpdateAsync(oficio);

            return true;
        }
    }
}

