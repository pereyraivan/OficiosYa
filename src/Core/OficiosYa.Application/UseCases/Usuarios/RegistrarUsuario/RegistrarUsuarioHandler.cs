using AutoMapper;
using BCrypt.Net;
using MediatR;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioCommand, UsuarioResponse?>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public RegistrarUsuarioHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioResponse?> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
        {
            // Verificar si el email ya existe
            if (await _usuarioRepository.EmailExistsAsync(request.Request.Email))
            {
                return null;
            }

            // Crear nuevo usuario
            var usuario = new Usuario
            {
                Nombre = request.Request.Nombre,
                Apellido = request.Request.Apellido,
                Email = request.Request.Email.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.Password),
                Telefono = request.Request.Telefono,
                Direccion = request.Request.Direccion,
                Ciudad = request.Request.Ciudad,
                Provincia = request.Request.Provincia,
                CodigoPostal = request.Request.CodigoPostal,
                FechaRegistro = DateTime.UtcNow,
                EmailVerificado = false,
                Activo = true
            };

            var usuarioCreado = await _usuarioRepository.AddAsync(usuario);

            return _mapper.Map<UsuarioResponse>(usuarioCreado);
        }
    }
}
