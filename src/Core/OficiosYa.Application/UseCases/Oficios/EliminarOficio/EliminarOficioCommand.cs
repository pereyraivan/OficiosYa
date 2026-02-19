using MediatR;

namespace OficiosYa.Application.UseCases.Oficios.EliminarOficio
{
    public class EliminarOficioCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

