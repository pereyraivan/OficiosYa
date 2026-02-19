using Microsoft.AspNetCore.Mvc;
using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.UseCases.Oficios.CrearOficio;
using OficiosYa.Application.UseCases.Oficios.ActualizarOficio;
using OficiosYa.Application.UseCases.Oficios.EliminarOficio;
using OficiosYa.Application.UseCases.Oficios.ObtenerOficios;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Web.Controllers.Api.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class OficiosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUsuarioRepository _usuarioRepository;

        public OficiosController(IMediator mediator, IUsuarioRepository usuarioRepository)
        {
            _mediator = mediator;
            _usuarioRepository = usuarioRepository;
        }

        private async Task<bool> EsAdminAsync()
        {
            // Por ahora, verificamos si hay un header con el email del admin
            // En producción, deberías usar JWT y verificar el rol
            var adminEmail = Request.Headers["X-Admin-Email"].FirstOrDefault();
            if (string.IsNullOrEmpty(adminEmail))
            {
                return false;
            }

            var usuario = await _usuarioRepository.GetByEmailAsync(adminEmail);
            return usuario != null && usuario.EsAdmin;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerOficios([FromQuery] bool soloActivos = true)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            var query = new ObtenerOficiosQuery { SoloActivos = soloActivos };
            var oficios = await _mediator.Send(query);
            return Ok(oficios);
        }

        [HttpPost]
        public async Task<IActionResult> CrearOficio([FromBody] CrearOficioRequest request)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CrearOficioCommand { Request = request };
            var oficio = await _mediator.Send(command);

            if (oficio == null)
            {
                return Conflict(new { message = "El nombre del oficio ya existe" });
            }

            return CreatedAtAction(nameof(ObtenerOficio), new { id = oficio.Id }, oficio);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerOficio(int id)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            var query = new ObtenerOficiosQuery { SoloActivos = false };
            var oficios = await _mediator.Send(query);
            var oficio = oficios.FirstOrDefault(o => o.Id == id);

            if (oficio == null)
            {
                return NotFound(new { message = "Oficio no encontrado" });
            }

            return Ok(oficio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarOficio(int id, [FromBody] ActualizarOficioRequest request)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.Id = id;
            var command = new ActualizarOficioCommand { Request = request };
            var oficio = await _mediator.Send(command);

            if (oficio == null)
            {
                return NotFound(new { message = "Oficio no encontrado o nombre duplicado" });
            }

            return Ok(oficio);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarOficio(int id)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            var command = new EliminarOficioCommand { Id = id };
            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound(new { message = "Oficio no encontrado" });
            }

            return NoContent();
        }
    }
}

