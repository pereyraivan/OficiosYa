using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;

namespace OficiosYa.Application.UseCases.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioCommand : IRequest<UsuarioResponse?>
    {
        public RegistrarUsuarioRequest Request { get; set; } = null!;
    }
}
