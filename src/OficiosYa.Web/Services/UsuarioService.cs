using BCrypt.Net;
using OficiosYa.Web.Interfaces;
using OficiosYa.Web.Models;
using OficiosYa.Web.Models.DTOs;

namespace OficiosYa.Web.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioResponse?> RegistrarUsuarioAsync(RegistrarUsuarioRequest request)
        {
            // Verificar si el email ya existe
            if (await _usuarioRepository.EmailExistsAsync(request.Email))
            {
                return null;
            }

            // Crear nuevo usuario
            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Ciudad = request.Ciudad,
                Provincia = request.Provincia,
                CodigoPostal = request.CodigoPostal,
                FechaRegistro = DateTime.UtcNow,
                EmailVerificado = false,
                Activo = true
            };

            var usuarioCreado = await _usuarioRepository.AddAsync(usuario);

            return MapToUsuarioResponse(usuarioCreado);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);
            
            if (usuario == null || !usuario.Activo)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Credenciales inválidas"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
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
                Usuario = MapToUsuarioResponse(usuario),
                Message = "Login exitoso"
            };
        }

        public async Task<UsuarioResponse?> ObtenerUsuarioPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            return usuario != null ? MapToUsuarioResponse(usuario) : null;
        }

        public async Task<bool> VerificarEmailDisponibleAsync(string email)
        {
            return !await _usuarioRepository.EmailExistsAsync(email);
        }

        public async Task<bool> VerificarEmailAsync(string email, string token)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null) return false;

            // Aquí implementarías la lógica de verificación de email
            // Por ahora, simplemente marcamos como verificado
            usuario.EmailVerificado = true;
            await _usuarioRepository.UpdateAsync(usuario);
            return true;
        }

        private UsuarioResponse MapToUsuarioResponse(Usuario usuario)
        {
            return new UsuarioResponse
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion,
                Ciudad = usuario.Ciudad,
                Provincia = usuario.Provincia,
                CodigoPostal = usuario.CodigoPostal,
                FechaRegistro = usuario.FechaRegistro,
                EmailVerificado = usuario.EmailVerificado,
                Activo = usuario.Activo
            };
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            // Implementación básica de JWT
            // En un proyecto real, usarías JWT Bearer tokens
            return $"jwt_token_for_user_{usuario.Id}_{DateTime.UtcNow.Ticks}";
        }
    }
}
