using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.UseCases.Usuarios.RegistrarUsuario;
using OficiosYa.Application.UseCases.Usuarios.Login;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Web.Services;

namespace OficiosYa.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly JwtTokenService _jwtTokenService;

        public UsuariosController(IMediator mediator, IUsuarioRepository usuarioRepository, IProfesionalRepository profesionalRepository, JwtTokenService jwtTokenService)
        {
            _mediator = mediator;
            _usuarioRepository = usuarioRepository;
            _profesionalRepository = profesionalRepository;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistrarUsuarioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new RegistrarUsuarioCommand { Request = request };
            var usuario = await _mediator.Send(command);
            
            if (usuario == null)
            {
                return Conflict(new { message = "El email ya está registrado" });
            }

            return CreatedAtAction(nameof(ObtenerUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new LoginCommand { Request = request };
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return Unauthorized(new { success = false, message = response.Message ?? "Credenciales inválidas" });
            }

            var token = _jwtTokenService.GenerateToken(
                response.Usuario!.Id,
                response.Usuario.Email,
                response.Usuario.EsAdmin,
                response.Usuario.EsProfesional);

            var u = response.Usuario;
            var usuarioParaFront = new
            {
                id = u.Id,
                nombre = u.NombreCompleto,
                email = u.Email,
                telefono = u.Telefono,
                fechaRegistro = u.FechaRegistro,
                esProfesional = u.EsProfesional,
                esAdmin = u.EsAdmin,
                activo = u.Activo
            };

            return Ok(new
            {
                data = new { usuario = usuarioParaFront, token },
                success = true,
                message = response.Message
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerUsuario(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> ObtenerPerfil()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var usuario = await _usuarioRepository.GetByIdAsync(userId);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            var profesional = await _profesionalRepository.GetByUsuarioIdAsync(usuario.Id);
            return Ok(new
            {
                data = new
                {
                    id = usuario.Id,
                    nombre = $"{usuario.Nombre} {usuario.Apellido}".Trim(),
                    email = usuario.Email,
                    telefono = usuario.Telefono,
                    fechaRegistro = usuario.FechaRegistro,
                    esProfesional = profesional != null,
                    esAdmin = usuario.EsAdmin,
                    activo = usuario.Activo,
                    profesionalId = profesional?.Id
                },
                success = true
            });
        }

        [HttpGet("verificar-email")]
        public async Task<IActionResult> VerificarEmailDisponible([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "El email es requerido" });
            }

            var existe = await _usuarioRepository.EmailExistsAsync(email);
            return Ok(new { disponible = !existe });
        }
    }
}
