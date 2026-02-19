using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.Usuarios.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public LoginRequest Request { get; set; } = null!;
    }
}
