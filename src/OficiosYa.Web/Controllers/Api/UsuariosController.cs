using Microsoft.AspNetCore.Mvc;
using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Application.UseCases.Usuarios.RegistrarUsuario;
using OficiosYa.Application.UseCases.Usuarios.Login;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuariosController(IMediator mediator, IUsuarioRepository usuarioRepository)
        {
            _mediator = mediator;
            _usuarioRepository = usuarioRepository;
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
                return Unauthorized(response);
            }

            return Ok(response);
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
