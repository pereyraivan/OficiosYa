using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.Profesionales.RegistrarProfesional
{
    public class RegistrarProfesionalCommand : IRequest<ProfesionalResponse?>
    {
        public RegistrarProfesionalRequest Request { get; set; } = null!;
    }
}

