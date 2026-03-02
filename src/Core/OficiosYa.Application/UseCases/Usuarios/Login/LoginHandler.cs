using AutoMapper;
using BCrypt.Net;
using MediatR;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Usuarios.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IMapper _mapper;

        public LoginHandler(IUsuarioRepository usuarioRepository, IProfesionalRepository profesionalRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _profesionalRepository = profesionalRepository;
            _mapper = mapper;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Request.Email);

            if (usuario == null || !usuario.Activo)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Credenciales inválidas"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Request.Password, usuario.PasswordHash))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Credenciales inválidas"
                };
            }

            var usuarioResponse = _mapper.Map<UsuarioResponse>(usuario);
            usuarioResponse.EsAdmin = usuario.EsAdmin;
            usuarioResponse.EsProfesional = (await _profesionalRepository.GetByUsuarioIdAsync(usuario.Id)) != null;

            return new LoginResponse
            {
                Success = true,
                Usuario = usuarioResponse,
                Message = "Login exitoso"
            };
        }
    }
}
