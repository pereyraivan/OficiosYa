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
        private readonly IMapper _mapper;

        public LoginHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
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

            // Aquí podrías generar un JWT token
            var token = GenerateJwtToken(usuario);

            return new LoginResponse
            {
                Success = true,
                Token = token,
                Usuario = _mapper.Map<UsuarioResponse>(usuario),
                Message = "Login exitoso"
            };
        }

        private string GenerateJwtToken(Domain.Entities.Usuario usuario)
        {
            // Implementación básica de JWT
            // En un proyecto real, usarías JWT Bearer tokens
            return $"jwt_token_for_user_{usuario.Id}_{DateTime.UtcNow.Ticks}";
        }
    }
}
