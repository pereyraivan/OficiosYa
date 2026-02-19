using MediatR;

namespace OficiosYa.Application.UseCases.CategoriaServicios.EliminarCategoriaServicio
{
    public class EliminarCategoriaServicioCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

