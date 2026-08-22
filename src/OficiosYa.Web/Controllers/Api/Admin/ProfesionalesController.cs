using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OficiosYa.Application.Common;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Persistence.Context;
using OficiosYa.Web.Contracts;

namespace OficiosYa.Web.Controllers.Api.Admin;

[ApiController]
[Route("api/admin/[controller]")]
public class ProfesionalesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IUsuarioRepository _usuarioRepository;

    public ProfesionalesController(ApplicationDbContext context, IUsuarioRepository usuarioRepository)
    {
        _context = context;
        _usuarioRepository = usuarioRepository;
    }

    private async Task<bool> EsAdminAsync()
    {
        var adminEmail = Request.Headers["X-Admin-Email"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(adminEmail))
            return false;

        var usuario = await _usuarioRepository.GetByEmailAsync(adminEmail.Trim());
        return usuario is { EsAdmin: true };
    }

    /// <summary>Listado de todos los profesionales y oficiantes registrados.</summary>
    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery] string? tipo = null,
        [FromQuery] bool? soloActivos = null,
        CancellationToken cancellationToken = default)
    {
        if (!await EsAdminAsync())
            return Unauthorized(new { message = "No autorizado" });

        var ahora = DateTime.UtcNow;
        var query = _context.Profesionales
            .AsNoTracking()
            .Include(p => p.Usuario)
            .Include(p => p.ProfesionalCategorias)
                .ThenInclude(pc => pc.CategoriaServicio)
                    .ThenInclude(c => c.TipoServicio)
            .Include(p => p.Suscripciones)
            .AsQueryable();

        if (soloActivos == true)
            query = query.Where(p => p.Activo);

        var tipoFiltro = tipo?.Trim();
        if (!string.IsNullOrEmpty(tipoFiltro) &&
            (tipoFiltro.Equals("Oficiante", StringComparison.OrdinalIgnoreCase) ||
             tipoFiltro.Equals("Profesional", StringComparison.OrdinalIgnoreCase)))
        {
            query = query.Where(p => p.ProfesionalCategorias.Any(pc =>
                pc.Activo &&
                pc.CategoriaServicio != null &&
                pc.CategoriaServicio.TipoServicio != null &&
                pc.CategoriaServicio.TipoServicio.Nombre == tipoFiltro));
        }

        var lista = await query
            .OrderByDescending(p => p.FechaRegistro)
            .ToListAsync(cancellationToken);

        return Ok(lista.Select(p => MapearDto(p, ahora)).ToList());
    }

    /// <summary>Detalle de un profesional/oficiante para edición admin.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obtener(int id, CancellationToken cancellationToken)
    {
        if (!await EsAdminAsync())
            return Unauthorized(new { message = "No autorizado" });

        var profesional = await _context.Profesionales
            .AsNoTracking()
            .Include(p => p.Usuario)
            .Include(p => p.ProfesionalCategorias)
                .ThenInclude(pc => pc.CategoriaServicio)
                    .ThenInclude(c => c.TipoServicio)
            .Include(p => p.Suscripciones)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (profesional == null)
            return NotFound(new { message = "Profesional no encontrado." });

        return Ok(MapearDto(profesional, DateTime.UtcNow));
    }

    /// <summary>Actualiza datos de usuario y perfil profesional (soporte admin).</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] AdminActualizarProfesionalRequest request, CancellationToken cancellationToken)
    {
        if (!await EsAdminAsync())
            return Unauthorized(new { message = "No autorizado" });

        if (request == null)
            return BadRequest(new { message = "Datos inválidos." });

        var nombre = (request.Nombre ?? string.Empty).Trim();
        var apellido = (request.Apellido ?? string.Empty).Trim();
        var email = (request.Email ?? string.Empty).Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest(new { message = "El nombre es obligatorio." });
        if (string.IsNullOrWhiteSpace(apellido))
            return BadRequest(new { message = "El apellido es obligatorio." });
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { message = "El email es obligatorio." });

        var profesional = await _context.Profesionales
            .Include(p => p.Usuario)
            .Include(p => p.ProfesionalCategorias)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (profesional == null)
            return NotFound(new { message = "Profesional no encontrado." });

        if (profesional.Usuario == null)
            return BadRequest(new { message = "El profesional no tiene usuario asociado." });

        var emailActual = (profesional.Usuario.Email ?? string.Empty).Trim().ToLowerInvariant();
        if (!string.Equals(email, emailActual, StringComparison.OrdinalIgnoreCase))
        {
            if (await _usuarioRepository.EmailExistsAsync(email))
                return Conflict(new { message = "Ya existe otro usuario con ese email." });
            profesional.Usuario.Email = email;
        }

        profesional.Usuario.Nombre = nombre;
        profesional.Usuario.Apellido = apellido;
        profesional.Usuario.Telefono = string.IsNullOrWhiteSpace(request.Telefono) ? null : request.Telefono.Trim();
        profesional.Usuario.Ciudad = string.IsNullOrWhiteSpace(request.Ciudad) ? null : request.Ciudad.Trim();
        profesional.Usuario.Provincia = string.IsNullOrWhiteSpace(request.Provincia) ? null : request.Provincia.Trim();

        if (!string.IsNullOrWhiteSpace(request.Documento))
        {
            var documento = DocumentoIdentidad.Normalizar(request.Documento);
            if (!DocumentoIdentidad.EsValido(documento))
                return BadRequest(new { message = "Ingresá un DNI (7 u 8 dígitos) o CUIT/CUIL (11 dígitos) válido." });

            var docActual = profesional.Usuario.Documento ?? string.Empty;
            if (!string.Equals(documento, docActual, StringComparison.Ordinal))
            {
                if (await _usuarioRepository.DocumentoExistsAsync(documento))
                    return Conflict(new { message = "Ya existe otro usuario con ese DNI/CUIT." });
                profesional.Usuario.Documento = documento;
            }
        }

        profesional.Descripcion = request.Descripcion?.Trim();
        profesional.Disponible = request.Disponible;
        profesional.Activo = request.Activo;

        if (request.Categorias != null)
        {
            foreach (var pc in profesional.ProfesionalCategorias)
                pc.Activo = false;

            var nombres = request.Categorias
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (nombres.Count > 0)
            {
                var categoriasDb = await _context.CategoriaServicios
                    .Where(c => c.Activo && nombres.Contains(c.Nombre))
                    .ToListAsync(cancellationToken);

                foreach (var cat in categoriasDb)
                {
                    var existente = profesional.ProfesionalCategorias
                        .FirstOrDefault(pc => pc.CategoriaServicioId == cat.Id);
                    if (existente == null)
                    {
                        _context.ProfesionalCategorias.Add(new ProfesionalCategoria
                        {
                            ProfesionalId = profesional.Id,
                            CategoriaServicioId = cat.Id,
                            FechaAsignacion = DateTime.UtcNow,
                            Activo = true
                        });
                    }
                    else
                    {
                        existente.Activo = true;
                    }
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var actualizado = await _context.Profesionales
            .AsNoTracking()
            .Include(p => p.Usuario)
            .Include(p => p.ProfesionalCategorias)
                .ThenInclude(pc => pc.CategoriaServicio)
                    .ThenInclude(c => c.TipoServicio)
            .Include(p => p.Suscripciones)
            .FirstAsync(p => p.Id == id, cancellationToken);

        return Ok(MapearDto(actualizado, DateTime.UtcNow));
    }

    /// <summary>Elimina (desactiva) un profesional/oficiante. No borra el historial.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        if (!await EsAdminAsync())
            return Unauthorized(new { message = "No autorizado" });

        var profesional = await _context.Profesionales
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (profesional == null)
            return NotFound(new { message = "Profesional no encontrado." });

        profesional.Activo = false;
        profesional.Disponible = false;

        await _context.SaveChangesAsync(cancellationToken);

        var nombre = profesional.Usuario != null
            ? $"{profesional.Usuario.Nombre} {profesional.Usuario.Apellido}".Trim()
            : $"#{id}";

        return Ok(new
        {
            message = $"El perfil de {nombre} fue eliminado (desactivado) y ya no aparece en búsquedas.",
            id
        });
    }

    private static AdminProfesionalDto MapearDto(Profesional p, DateTime ahora)
    {
        var categoriasActivas = p.ProfesionalCategorias
            .Where(pc => pc.Activo && pc.CategoriaServicio != null)
            .ToList();

        var tipos = categoriasActivas
            .Select(pc => pc.CategoriaServicio!.TipoServicio?.Nombre)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(n => n)
            .Cast<string>()
            .ToList();

        var categorias = categoriasActivas
            .Select(pc => pc.CategoriaServicio!.Nombre)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(n => n)
            .ToList();

        var suscripcionActiva = p.Suscripciones?
            .FirstOrDefault(s => s.Estado == "Activa" && s.FechaVencimiento > ahora);
        var suscripcionPendiente = p.Suscripciones?
            .FirstOrDefault(s => s.Estado == "Pendiente" && !s.PagoConfirmado);

        var u = p.Usuario;
        return new AdminProfesionalDto
        {
            Id = p.Id,
            UsuarioId = p.UsuarioId,
            Nombre = u?.Nombre ?? string.Empty,
            Apellido = u?.Apellido ?? string.Empty,
            Email = u?.Email ?? string.Empty,
            Telefono = u?.Telefono,
            Documento = u?.Documento,
            Ciudad = u?.Ciudad,
            Provincia = u?.Provincia,
            Descripcion = p.Descripcion,
            Tipos = tipos,
            Categorias = categorias,
            PlanActual = suscripcionActiva?.TipoSuscripcion ?? "Free",
            SuscripcionEstado = suscripcionActiva != null
                ? "Activa"
                : suscripcionPendiente != null
                    ? "Pendiente"
                    : null,
            Disponible = p.Disponible,
            Activo = p.Activo,
            CalificacionPromedio = p.CalificacionPromedio,
            TotalResenas = p.TotalResenas,
            FechaRegistro = p.FechaRegistro
        };
    }
}
