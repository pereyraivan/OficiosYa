using Microsoft.AspNetCore.Mvc;
using MediatR;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.UseCases.CategoriaServicios.CrearCategoriaServicio;
using OficiosYa.Application.UseCases.CategoriaServicios.ActualizarCategoriaServicio;
using OficiosYa.Application.UseCases.CategoriaServicios.EliminarCategoriaServicio;
using OficiosYa.Application.UseCases.CategoriaServicios.ObtenerCategoriaServicios;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Web.Controllers.Api.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class CategoriaServiciosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUsuarioRepository _usuarioRepository;

        public CategoriaServiciosController(IMediator mediator, IUsuarioRepository usuarioRepository)
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
        public async Task<IActionResult> ObtenerCategoriaServicios([FromQuery] string? tipo = null, [FromQuery] bool soloActivos = true)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            var query = new ObtenerCategoriaServiciosQuery { Tipo = tipo, SoloActivos = soloActivos };
            var categorias = await _mediator.Send(query);
            return Ok(categorias);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCategoriaServicio([FromBody] CrearCategoriaServicioRequest request)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CrearCategoriaServicioCommand { Request = request };
            var categoria = await _mediator.Send(command);

            if (categoria == null)
            {
                return Conflict(new { message = "El nombre de la categoría ya existe para este tipo o el tipo es inválido" });
            }

            return CreatedAtAction(nameof(ObtenerCategoriaServicio), new { id = categoria.Id }, categoria);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCategoriaServicio(int id)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            var query = new ObtenerCategoriaServiciosQuery { SoloActivos = false };
            var categorias = await _mediator.Send(query);
            var categoria = categorias.FirstOrDefault(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound(new { message = "Categoría no encontrada" });
            }

            return Ok(categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoriaServicio(int id, [FromBody] ActualizarCategoriaServicioRequest request)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            request.Id = id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new ActualizarCategoriaServicioCommand { Request = request };
            var categoria = await _mediator.Send(command);

            if (categoria == null)
            {
                return NotFound(new { message = "Categoría no encontrada, nombre duplicado o tipo inválido" });
            }

            return Ok(categoria);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoriaServicio(int id)
        {
            if (!await EsAdminAsync())
            {
                return Unauthorized(new { message = "No autorizado" });
            }

            var command = new EliminarCategoriaServicioCommand { Id = id };
            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound(new { message = "Categoría no encontrada" });
            }

            return NoContent();
        }
    }
}

