using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OficiosYa.Persistence.Context;
using OficiosYa.Domain.Entities;

namespace OficiosYa.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrabajosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrabajosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Publicar un empleo/trabajo. El cliente indica qué servicios necesita y datos de contacto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Publicar([FromBody] PublicarTrabajoRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Datos del trabajo son requeridos." });

            if (string.IsNullOrWhiteSpace(request.Descripcion))
                return BadRequest(new { message = "La descripción del trabajo es obligatoria." });

            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { message = "El email de contacto es obligatorio." });

            if (request.CategoriaIds == null || request.CategoriaIds.Count == 0)
                return BadRequest(new { message = "Debe seleccionar al menos un tipo de servicio (oficio o profesión)." });

            var categoriasExistentes = await _context.CategoriaServicios
                .Where(c => c.Activo && request.CategoriaIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            if (categoriasExistentes.Count != request.CategoriaIds.Distinct().Count())
                return BadRequest(new { message = "Algunas categorías seleccionadas no son válidas." });

            var trabajo = new TrabajoPublicado
            {
                Descripcion = request.Descripcion.Trim(),
                Telefono = request.Telefono?.Trim(),
                Email = request.Email.Trim(),
                Estado = "Abierto",
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            _context.TrabajosPublicados.Add(trabajo);
            await _context.SaveChangesAsync();

            foreach (var catId in categoriasExistentes)
            {
                _context.TrabajoCategorias.Add(new TrabajoCategoria
                {
                    TrabajoPublicadoId = trabajo.Id,
                    CategoriaServicioId = catId
                });
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerUno), new { id = trabajo.Id }, new
            {
                success = true,
                data = new
                {
                    id = trabajo.Id,
                    descripcion = trabajo.Descripcion,
                    telefono = trabajo.Telefono,
                    email = trabajo.Email,
                    estado = trabajo.Estado,
                    fechaCreacion = trabajo.FechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    categoriaIds = categoriasExistentes
                }
            });
        }

        /// <summary>
        /// Listar empleos publicados para que oficiantes/profesionales busquen. Filtro por categorías y estado.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] string? estado = "Abierto",
            [FromQuery] string? categoriaIds = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var query = _context.TrabajosPublicados
                .Include(t => t.TrabajoCategorias)
                    .ThenInclude(tc => tc.CategoriaServicio)
                .AsNoTracking()
                .Where(t => t.Activo);

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(t => t.Estado == estado.Trim());

            if (!string.IsNullOrWhiteSpace(categoriaIds))
            {
                var ids = categoriaIds.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(s => int.TryParse(s, out var n) ? n : (int?)null)
                    .Where(n => n.HasValue)
                    .Select(n => n!.Value)
                    .Distinct()
                    .ToList();
                if (ids.Count > 0)
                {
                    query = query.Where(t => t.TrabajoCategorias.Any(tc => ids.Contains(tc.CategoriaServicioId)));
                }
            }

            var total = await query.CountAsync();
            var trabajos = await query
                .OrderByDescending(t => t.FechaCreacion)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = trabajos.Select(t => new
            {
                id = t.Id,
                descripcionResumen = t.Descripcion.Length > 200 ? t.Descripcion.Substring(0, 197) + "..." : t.Descripcion,
                descripcion = t.Descripcion,
                estado = t.Estado,
                fechaCreacion = t.FechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                categorias = t.TrabajoCategorias
                    .Where(tc => tc.CategoriaServicio != null)
                    .Select(tc => new { id = tc.CategoriaServicio!.Id, nombre = tc.CategoriaServicio.Nombre })
                    .ToList()
            }).ToList();

            return Ok(new
            {
                success = true,
                data = new
                {
                    items,
                    total,
                    page,
                    pageSize,
                    totalPaginas = (int)Math.Ceiling(total / (double)pageSize)
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerUno(int id)
        {
            var trabajo = await _context.TrabajosPublicados
                .Include(t => t.TrabajoCategorias)
                    .ThenInclude(tc => tc.CategoriaServicio)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.Activo);

            if (trabajo == null)
                return NotFound(new { message = "Trabajo no encontrado." });

            return Ok(new
            {
                success = true,
                data = new
                {
                    id = trabajo.Id,
                    descripcion = trabajo.Descripcion,
                    telefono = trabajo.Telefono,
                    email = trabajo.Email,
                    estado = trabajo.Estado,
                    fechaCreacion = trabajo.FechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    categorias = trabajo.TrabajoCategorias
                        .Where(tc => tc.CategoriaServicio != null)
                        .Select(tc => new { id = tc.CategoriaServicio!.Id, nombre = tc.CategoriaServicio.Nombre })
                        .ToList()
                }
            });
        }
    }

    public class PublicarTrabajoRequest
    {
        public string Descripcion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<int> CategoriaIds { get; set; } = new List<int>();
    }
}
