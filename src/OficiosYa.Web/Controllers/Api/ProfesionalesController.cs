using Microsoft.AspNetCore.Mvc;
using OficiosYa.Web.Interfaces;
using OficiosYa.Web.Models.DTOs;

namespace OficiosYa.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesionalesController : ControllerBase
    {
        private readonly IProfesionalService _profesionalService;

        public ProfesionalesController(IProfesionalService profesionalService)
        {
            _profesionalService = profesionalService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarProfesional([FromBody] RegistrarProfesionalRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profesional = await _profesionalService.RegistrarProfesionalAsync(request);
            
            if (profesional == null)
            {
                return Conflict(new { message = "El email ya está registrado" });
            }

            return CreatedAtAction(nameof(ObtenerProfesional), new { id = profesional.Id }, profesional);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerProfesional(int id)
        {
            var profesional = await _profesionalService.ObtenerProfesionalPorIdAsync(id);
            
            if (profesional == null)
            {
                return NotFound();
            }

            return Ok(new { data = profesional, success = true });
        }

        [HttpGet("destacados")]
        public async Task<IActionResult> ObtenerDestacados()
        {
            var profesionales = await _profesionalService.ObtenerProfesionalesDestacadosAsync();
            return Ok(new { data = profesionales, success = true });
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarProfesionales([FromQuery] string oficio, [FromQuery] string zona, [FromQuery] int radio = 10)
        {
            if (string.IsNullOrEmpty(oficio))
            {
                return BadRequest(new { message = "El oficio es requerido" });
            }

            var profesionales = await _profesionalService.BuscarProfesionalesAsync(oficio, zona, radio);
            return Ok(new { data = profesionales, success = true });
        }

        [HttpGet("oficio/{oficio}")]
        public async Task<IActionResult> ObtenerPorOficio(string oficio)
        {
            var profesionales = await _profesionalService.ObtenerProfesionalesPorOficioAsync(oficio);
            return Ok(new { data = profesionales, success = true });
        }

        [HttpGet("cerca")]
        public async Task<IActionResult> ObtenerCercaDe([FromQuery] string ciudad, [FromQuery] int radio = 10)
        {
            if (string.IsNullOrEmpty(ciudad))
            {
                return BadRequest(new { message = "La ciudad es requerida" });
            }

            var profesionales = await _profesionalService.ObtenerProfesionalesCercaDeAsync(ciudad, radio);
            return Ok(new { data = profesionales, success = true });
        }

        [HttpPut("{id}/disponibilidad")]
        public async Task<IActionResult> ActualizarDisponibilidad(int id, [FromBody] bool disponible)
        {
            var exito = await _profesionalService.ActualizarDisponibilidadAsync(id, disponible);
            
            if (!exito)
            {
                return NotFound();
            }

            return Ok(new { message = "Disponibilidad actualizada correctamente", success = true });
        }

        [HttpPut("{id}/perfil")]
        public async Task<IActionResult> ActualizarPerfil(int id, [FromBody] ActualizarPerfilRequest request)
        {
            var exito = await _profesionalService.ActualizarPerfilAsync(id, request.Descripcion, request.FotoPerfil);
            
            if (!exito)
            {
                return NotFound();
            }

            return Ok(new { message = "Perfil actualizado correctamente", success = true });
        }
    }

    public class ActualizarPerfilRequest
    {
        public string? Descripcion { get; set; }
        public string? FotoPerfil { get; set; }
    }
}
