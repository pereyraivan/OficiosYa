using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.Profesionales.SolicitarSuscripcion
{
    public class SolicitarSuscripcionCommand : IRequest<SolicitarSuscripcionResponse>
    {
        public int UsuarioId { get; set; }

        public SolicitarSuscripcionRequest Request { get; set; } = null!;
    }
}
