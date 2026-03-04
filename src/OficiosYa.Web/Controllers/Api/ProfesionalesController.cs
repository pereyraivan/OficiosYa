using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.UseCases.Profesionales.RegistrarProfesional;
using OficiosYa.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Data.Common;
using System.Security.Claims;
using OficiosYa.Domain.Entities;
using OficiosYa.Web.Services;

namespace OficiosYa.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesionalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IOpenAIModerationService _moderation;

        public ProfesionalesController(IMediator mediator, ApplicationDbContext context, IWebHostEnvironment environment, IOpenAIModerationService moderation)
        {
            _mediator = mediator;
            _context = context;
            _environment = environment;
            _moderation = moderation;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarProfesional([FromBody] RegistrarProfesionalRequest request)
        {
            if (!ModelState.IsValid)
            {
                string? primerError = null;
                foreach (var entry in ModelState.Values)
                {
                    if (entry?.Errors == null) continue;
                    foreach (var err in entry.Errors)
                    {
                        if (!string.IsNullOrEmpty(err.ErrorMessage))
                        {
                            primerError = err.ErrorMessage;
                            break;
                        }
                    }
                    if (primerError != null) break;
                }
                return BadRequest(new { message = primerError ?? "Revisá los datos del formulario.", errors = ModelState });
            }

            if (_moderation.IsEnabled)
            {
                var textos = new List<string>();
                if (!string.IsNullOrWhiteSpace(request.Descripcion)) textos.Add(request.Descripcion);
                var imagenes = new List<string>();
                if (!string.IsNullOrWhiteSpace(request.FotoPerfilBase64)) imagenes.Add(request.FotoPerfilBase64);
                if (request.ImagenesTrabajoBase64 != null) imagenes.AddRange(request.ImagenesTrabajoBase64.Where(x => !string.IsNullOrWhiteSpace(x)));
                var (aprobado, mensaje) = await _moderation.ModerateAsync(textos: textos.Count > 0 ? textos : null, imagenesBase64: imagenes.Count > 0 ? imagenes : null);
                if (!aprobado)
                    return BadRequest(new { message = mensaje });
            }

            try
            {
                var command = new RegistrarProfesionalCommand { Request = request };
                var profesional = await _mediator.Send(command);
                
                if (profesional == null)
                {
                    return Conflict(new { message = "El email ya está registrado" });
                }

                if (!string.IsNullOrWhiteSpace(request.FotoPerfilBase64))
                {
                    var urlFoto = await GuardarFotoPerfilAsync(profesional.Id, request.FotoPerfilBase64);
                    if (!string.IsNullOrEmpty(urlFoto))
                    {
                        var prof = await _context.Profesionales.FindAsync(profesional.Id);
                        if (prof != null)
                        {
                            prof.FotoPerfil = urlFoto;
                            await _context.SaveChangesAsync();
                        }
                        profesional.FotoPerfil = urlFoto;
                    }
                }

                if (request.ImagenesTrabajoBase64 != null && request.ImagenesTrabajoBase64.Count > 0)
                {
                    await GuardarImagenesTrabajoAsync(profesional.Id, request.ImagenesTrabajoBase64);
                }

                return CreatedAtAction(nameof(ObtenerProfesional), new { id = profesional.Id }, profesional);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Incluir el error interno para debugging
                var innerException = ex.InnerException != null 
                    ? ex.InnerException.Message 
                    : string.Empty;
                
                return StatusCode(500, new 
                { 
                    message = "Error al registrar profesional", 
                    error = ex.Message,
                    innerError = innerException,
                    stackTrace = _environment.IsDevelopment() ? ex.StackTrace : null
                });
            }
        }

        /// <summary>
        /// Valida imágenes (moderación) sin crear registro. Para usar al subir foto de perfil, fotos de trabajo o certificados.
        /// </summary>
        [HttpPost("validar-imagenes")]
        public async Task<IActionResult> ValidarImagenes([FromBody] ValidarImagenesRequest request)
        {
            if (request?.ImagenesBase64 == null || request.ImagenesBase64.Count == 0)
                return BadRequest(new { message = "No se enviaron imágenes para validar." });
            var list = request.ImagenesBase64.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (list.Count == 0)
                return BadRequest(new { message = "No se enviaron imágenes para validar." });
            var (aprobado, mensaje) = await _moderation.ModerateAsync(textos: null, imagenesBase64: list);
            if (!aprobado)
                return BadRequest(new { message = mensaje });
            return Ok(new { aprobado = true });
        }

        /// <summary>
        /// Obtiene el perfil del profesional asociado al usuario autenticado (para el panel del profesional).
        /// </summary>
        [HttpGet("mi-perfil")]
        [Authorize]
        public async Task<IActionResult> ObtenerMiPerfil()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var profesional = await _context.Profesionales
                .Include(p => p.Usuario)
                .Include(p => p.ProfesionalCategorias)
                    .ThenInclude(pc => pc.CategoriaServicio)
                .Include(p => p.ZonasCobertura)
                .Include(p => p.ImagenesTrabajo)
                .Include(p => p.Suscripciones)
                .FirstOrDefaultAsync(p => p.UsuarioId == userId);

            if (profesional == null)
            {
                return NotFound(new { message = "No tenés un perfil de profesional asociado a esta cuenta." });
            }

            // Marcar suscripciones vencidas (Estado Activa pero FechaVencimiento ya pasó) para mantener consistencia
            var vencidas = profesional.Suscripciones.Where(s => s.Estado == "Activa" && s.FechaVencimiento <= DateTime.UtcNow).ToList();
            foreach (var s in vencidas)
                s.MarcarComoVencida();
            if (vencidas.Count > 0)
                await _context.SaveChangesAsync();

            var suscripcionActiva = profesional.Suscripciones
                .FirstOrDefault(s => s.Estado == "Activa" && s.FechaVencimiento > DateTime.UtcNow);
            var planActual = suscripcionActiva?.TipoSuscripcion ?? "Free";
            var limiteContactos = PlanLimitsHelper.GetLimiteContactosMensuales(planActual);
            var inicioMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var finMes = inicioMes.AddMonths(1);
            var contactosUsadosEsteMes = await _context.ContactosProfesional
                .CountAsync(c => c.ProfesionalId == profesional.Id && c.FechaHora >= inicioMes && c.FechaHora < finMes);
            var accesoEmpleos = PlanLimitsHelper.TieneAccesoEmpleos(planActual);

            var u = profesional.Usuario;
            return Ok(new
            {
                success = true,
                data = new
                {
                    id = profesional.Id,
                    usuarioId = profesional.UsuarioId,
                    descripcion = profesional.Descripcion ?? string.Empty,
                    fotoPerfil = profesional.FotoPerfil,
                    calificacionPromedio = profesional.CalificacionPromedio,
                    totalResenas = profesional.TotalResenas,
                    disponible = profesional.Disponible,
                    fechaRegistro = profesional.FechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    activo = profesional.Activo,
                    usuario = u != null ? new
                    {
                        id = u.Id,
                        nombre = (u.Nombre ?? "") + " " + (u.Apellido ?? "").Trim(),
                        email = u.Email ?? "",
                        telefono = u.Telefono,
                        ciudad = u.Ciudad,
                        provincia = u.Provincia
                    } : null,
                    oficios = profesional.ProfesionalCategorias
                        .Where(pc => pc.Activo && pc.CategoriaServicio != null)
                        .Select(pc => new { id = pc.CategoriaServicioId, nombre = pc.CategoriaServicio!.Nombre ?? "" })
                        .ToList(),
                    zonasCobertura = profesional.ZonasCobertura
                        .Where(z => z.Activo)
                        .Select(z => new
                        {
                            localidad = z.Localidad,
                            provincia = z.Provincia,
                            departamento = z.Departamento,
                            latitud = z.Latitud,
                            longitud = z.Longitud,
                            codigoPostal = z.CodigoPostal
                        })
                        .ToList(),
                    imagenesTrabajo = profesional.ImagenesTrabajo
                        .Where(i => i.Activo)
                        .OrderBy(i => i.Orden)
                        .ThenBy(i => i.Id)
                        .Select(i => new { id = i.Id, urlImagen = i.UrlImagen, titulo = i.Titulo, descripcion = i.Descripcion })
                        .ToList(),
                    suscripcionActiva = suscripcionActiva != null ? new
                    {
                        tipo = suscripcionActiva.TipoSuscripcion,
                        fechaVencimiento = suscripcionActiva.FechaVencimiento.ToString("yyyy-MM-dd"),
                        estado = suscripcionActiva.Estado
                    } : null,
                    planActual,
                    limiteContactosMensual = limiteContactos,
                    contactosUsadosEsteMes,
                    accesoEmpleos,
                    maxZonasCobertura = PlanLimitsHelper.GetMaxZonasCobertura(planActual)
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerProfesional(int id)
        {
            var profesional = await _context.Profesionales
                .Include(p => p.Usuario)
                .Include(p => p.Suscripciones)
                .Include(p => p.ProfesionalCategorias)
                    .ThenInclude(pc => pc.CategoriaServicio)
                .Include(p => p.ZonasCobertura)
                .Include(p => p.ImagenesTrabajo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profesional == null)
            {
                return NotFound(new { message = "Profesional no encontrado" });
            }

            var planActual = ObtenerPlanActualDelProfesional(profesional);
            var limite = PlanLimitsHelper.GetLimiteContactosMensuales(planActual);
            var inicioMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var finMes = inicioMes.AddMonths(1);
            var contactosEsteMes = await _context.ContactosProfesional
                .CountAsync(c => c.ProfesionalId == id && c.FechaHora >= inicioMes && c.FechaHora < finMes);
            var puedeRecibirContactos = contactosEsteMes < limite;
            var mensajeLimiteContactos = puedeRecibirContactos ? null : "Este profesional alcanzó el límite de contactos de su plan este mes. No está disponible para nuevos contactos hasta el próximo mes.";

            var u = profesional.Usuario;
            return Ok(new
            {
                success = true,
                data = new
                {
                    id = profesional.Id,
                    usuarioId = profesional.UsuarioId,
                    descripcion = profesional.Descripcion ?? string.Empty,
                    fotoPerfil = profesional.FotoPerfil,
                    calificacionPromedio = profesional.CalificacionPromedio,
                    totalResenas = profesional.TotalResenas,
                    disponible = profesional.Disponible,
                    fechaRegistro = profesional.FechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    activo = profesional.Activo,
                    puedeRecibirContactos,
                    mensajeLimiteContactos,
                    usuario = u != null ? new
                    {
                        id = u.Id,
                        nombre = (u.Nombre ?? "") + " " + (u.Apellido ?? "").Trim(),
                        email = puedeRecibirContactos ? (u.Email ?? "") : null,
                        telefono = puedeRecibirContactos ? u.Telefono : null,
                        ciudad = u.Ciudad,
                        provincia = u.Provincia
                    } : null,
                    oficios = profesional.ProfesionalCategorias
                        .Where(pc => pc.Activo && pc.CategoriaServicio != null)
                        .Select(pc => new { nombre = pc.CategoriaServicio!.Nombre ?? "" })
                        .ToList(),
                    zonasCobertura = profesional.ZonasCobertura
                        .Where(z => z.Activo)
                        .Select(z => new { localidad = z.Localidad, provincia = z.Provincia, departamento = z.Departamento })
                        .ToList(),
                    imagenesTrabajo = profesional.ImagenesTrabajo
                        .Where(i => i.Activo)
                        .OrderBy(i => i.Orden)
                        .ThenBy(i => i.Id)
                        .Select(i => new { id = i.Id, urlImagen = i.UrlImagen, titulo = i.Titulo, descripcion = i.Descripcion })
                        .ToList()
                }
            });
        }

        /// <summary>
        /// Lista las reseñas de un profesional (cualquier usuario puede ver).
        /// </summary>
        [HttpGet("{id}/resenas")]
        public async Task<IActionResult> ListarResenas(int id)
        {
            var profesional = await _context.Profesionales.FindAsync(id);
            if (profesional == null)
                return NotFound(new { message = "Profesional no encontrado" });

            var resenas = await _context.Resenas
                .Where(r => r.ProfesionalId == id && r.Activa)
                .OrderByDescending(r => r.FechaCreacion)
                .Include(r => r.Usuario)
                .Select(r => new
                {
                    id = r.Id,
                    profesionalId = r.ProfesionalId,
                    usuarioId = r.UsuarioId,
                    calificacion = r.Calificacion,
                    comentario = r.Comentario,
                    fechaCreacion = r.FechaCreacion.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    nombreAutor = r.Usuario != null
                        ? (r.Usuario.Nombre ?? "") + " " + (r.Usuario.Apellido ?? "").Trim()
                        : (r.AutorNombre ?? "Anónimo"),
                    usuario = r.Usuario != null ? new { id = r.Usuario.Id, nombre = (r.Usuario.Nombre ?? "") + " " + (r.Usuario.Apellido ?? "").Trim() } : null
                })
                .ToListAsync();

            return Ok(resenas);
        }

        /// <summary>
        /// Crea una reseña para el profesional. No requiere login: se puede enviar AutorNombre + Calificacion + Comentario.
        /// Si el usuario está logueado, se guarda su cuenta y no puede reseñar su propio perfil.
        /// </summary>
        [HttpPost("{id}/resenas")]
        public async Task<IActionResult> CrearResena(int id, [FromBody] CrearResenaRequest request)
        {
            if (request == null || request.Calificacion < 1 || request.Calificacion > 5)
                return BadRequest(new { message = "La calificación debe ser entre 1 y 5" });
            if (string.IsNullOrWhiteSpace(request.Comentario) || request.Comentario.Length > 1000)
                return BadRequest(new { message = "El comentario es obligatorio y no puede superar 1000 caracteres" });

            // Siempre moderar reseñas: filtro local de palabras (insultos/profanidad). Si hay API key de OpenAI, además se usa la API.
            var textosResena = new[] { request.Comentario.Trim(), request.AutorNombre?.Trim() ?? "" }.Where(x => !string.IsNullOrEmpty(x)).ToList();
            var (aprobado, mensaje) = await _moderation.ModerateAsync(textos: textosResena.Count > 0 ? textosResena : null);
            if (!aprobado)
                return BadRequest(new { message = mensaje });

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            int? usuarioId = null;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var uid))
                usuarioId = uid;

            var profesional = await _context.Profesionales.FindAsync(id);
            if (profesional == null)
                return NotFound(new { message = "Profesional no encontrado" });
            if (usuarioId.HasValue && profesional.UsuarioId == usuarioId.Value)
                return BadRequest(new { message = "No podés dejar una reseña en tu propio perfil" });

            string? autorNombre = request.AutorNombre?.Trim();
            if (string.IsNullOrWhiteSpace(autorNombre))
            {
                if (!usuarioId.HasValue)
                    return BadRequest(new { message = "Indicá cómo querés que aparezca tu nombre en la reseña" });
                var usuario = await _context.Usuarios.FindAsync(usuarioId.Value);
                autorNombre = usuario != null ? (usuario.Nombre ?? "") + " " + (usuario.Apellido ?? "").Trim() : "Usuario";
            }
            if (autorNombre.Length > 100)
                autorNombre = autorNombre.Substring(0, 100);

            var resena = new Resena
            {
                ProfesionalId = id,
                UsuarioId = usuarioId,
                AutorNombre = autorNombre,
                Calificacion = request.Calificacion,
                Comentario = request.Comentario?.Trim() ?? "",
                FechaCreacion = DateTime.UtcNow,
                Activa = true
            };
            _context.Resenas.Add(resena);
            await _context.SaveChangesAsync();

            var activas = await _context.Resenas
                .Where(r => r.ProfesionalId == id && r.Activa)
                .ToListAsync();
            var total = activas.Count;
            var promedio = total > 0 ? (decimal)activas.Average(r => r.Calificacion) : 0;
            profesional.ActualizarCalificacion(promedio, total);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarResenas), new { id }, new
            {
                id = resena.Id,
                profesionalId = resena.ProfesionalId,
                usuarioId = resena.UsuarioId,
                calificacion = resena.Calificacion,
                comentario = resena.Comentario,
                fechaCreacion = resena.FechaCreacion.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                nombreAutor = autorNombre
            });
        }

        /// <summary>
        /// Actualiza el perfil del profesional (descripción, disponibilidad, foto, zonas, categorías).
        /// Solo el dueño del perfil puede actualizarlo.
        /// </summary>
        [HttpPut("mi-perfil")]
        [Authorize]
        public async Task<IActionResult> ActualizarMiPerfil([FromBody] ActualizarMiPerfilRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var profesional = await _context.Profesionales
                .Include(p => p.ProfesionalCategorias)
                .Include(p => p.ZonasCobertura)
                .Include(p => p.Suscripciones)
                .FirstOrDefaultAsync(p => p.UsuarioId == userId);

            if (profesional == null)
            {
                return NotFound(new { message = "No tenés un perfil de profesional asociado a esta cuenta." });
            }

            if (request.ZonasCobertura != null)
            {
                var planActual = ObtenerPlanActualDelProfesional(profesional);
                var maxZonas = PlanLimitsHelper.GetMaxZonasCobertura(planActual);
                if (request.ZonasCobertura.Count > maxZonas)
                {
                    return BadRequest(new
                    {
                        message = maxZonas == 1
                            ? "Con el plan Free solo podés tener una zona de cobertura. Actualizá tu plan para agregar más."
                            : $"Superás el límite de zonas permitidas para tu plan ({request.ZonasCobertura.Count} zonas)."
                    });
                }
            }

            if (_moderation.IsEnabled)
            {
                var textos = request.Descripcion != null ? new[] { request.Descripcion } : Array.Empty<string>();
                var imagenes = !string.IsNullOrWhiteSpace(request.FotoPerfilBase64) ? new[] { request.FotoPerfilBase64 } : Array.Empty<string>();
                var (aprobado, mensaje) = await _moderation.ModerateAsync(textos: textos.Length > 0 ? textos : null, imagenesBase64: imagenes.Length > 0 ? imagenes : null);
                if (!aprobado)
                    return BadRequest(new { message = mensaje });
            }

            if (request.Descripcion != null)
                profesional.Descripcion = request.Descripcion;

            if (request.Disponible.HasValue)
                profesional.Disponible = request.Disponible.Value;

            if (!string.IsNullOrWhiteSpace(request.FotoPerfilBase64))
            {
                var urlFoto = await GuardarFotoPerfilAsync(profesional.Id, request.FotoPerfilBase64);
                if (!string.IsNullOrEmpty(urlFoto))
                    profesional.FotoPerfil = urlFoto;
            }

            if (request.ZonasCobertura != null)
            {
                foreach (var z in profesional.ZonasCobertura)
                    z.Activo = false;
                foreach (var dto in request.ZonasCobertura)
                {
                    _context.ZonasCobertura.Add(new ZonaCobertura
                    {
                        ProfesionalId = profesional.Id,
                        Localidad = dto.Localidad ?? "",
                        Provincia = dto.Provincia ?? "",
                        Departamento = dto.Departamento,
                        Latitud = dto.Latitud,
                        Longitud = dto.Longitud,
                        CodigoPostal = dto.CodigoPostal,
                        FechaRegistro = DateTime.UtcNow,
                        Activo = true
                    });
                }
            }

            if (request.Oficios != null)
            {
                foreach (var pc in profesional.ProfesionalCategorias)
                    pc.Activo = false;
                var categorias = await _context.CategoriaServicios
                    .Where(c => c.Activo && request.Oficios.Contains(c.Nombre))
                    .ToListAsync();
                foreach (var cat in categorias)
                {
                    var existe = profesional.ProfesionalCategorias.Any(pc => pc.CategoriaServicioId == cat.Id);
                    if (!existe)
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
                        var pc = profesional.ProfesionalCategorias.First(p => p.CategoriaServicioId == cat.Id);
                        pc.Activo = true;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Perfil actualizado correctamente." });
        }

        [HttpGet("planes-suscripcion")]
        public async Task<IActionResult> ObtenerPlanesSuscripcion()
        {
            try
            {
                var planes = await _context.PlanesSuscripcion
                    .Where(p => p.Activo)
                    .OrderBy(p => p.PrecioMensual)
                    .Select(p => new
                    {
                        id = p.Id,
                        nombre = p.Nombre,
                        descripcion = p.Descripcion,
                        precioMensual = p.PrecioMensual,
                        precioTrimestral = p.PrecioTrimestral,
                        precioSemestral = p.PrecioSemestral,
                        precioAnual = p.PrecioAnual,
                        descuentoTrimestral = p.DescuentoTrimestral,
                        descuentoSemestral = p.DescuentoSemestral,
                        descuentoAnual = p.DescuentoAnual
                    })
                    .ToListAsync();

                return Ok(planes);
            }
            catch (Microsoft.Data.SqlClient.SqlException sqlEx) when (sqlEx.Message.Contains("Invalid object name") && sqlEx.Message.Contains("PlanesSuscripcion"))
            {
                // Error específico: tabla PlanesSuscripcion no existe
                return StatusCode(500, new
                {
                    message = "Error: La tabla PlanesSuscripcion no existe en la base de datos",
                    error = sqlEx.Message,
                    solution = "Ejecuta el script SQL: scripts/VerificarYCompletarTablas.sql o aplica las migraciones con: dotnet ef database update --project src/Infrastructure/OficiosYa.Persistence --startup-project src/OficiosYa.Web"
                });
            }
            catch (Exception ex)
            {
                // Log del error completo para debugging
                var innerException = ex.InnerException?.Message ?? string.Empty;
                return StatusCode(500, new 
                { 
                    message = "Error al obtener planes de suscripción", 
                    error = ex.Message,
                    innerError = innerException,
                    hint = "Verifica que la tabla PlanesSuscripcion existe en la base de datos. Ejecuta el script VerificarYCompletarTablas.sql o aplica las migraciones con: dotnet ef database update",
                    stackTrace = _environment.IsDevelopment() ? ex.StackTrace : null
                });
            }
        }

        /// <summary>
        /// Indica si el profesional autenticado puede postularse a empleos (solo plan Premium).
        /// Si no está autenticado o no es profesional, puede = false.
        /// </summary>
        [HttpGet("yo-puedo-postular-empleos")]
        [Authorize]
        public async Task<IActionResult> YoPuedoPostularEmpleos()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Ok(new { puedePostularEmpleos = false, planActual = "Free", message = "No autenticado" });
            }

            var profesional = await _context.Profesionales
                .Include(p => p.Suscripciones)
                .FirstOrDefaultAsync(p => p.UsuarioId == userId);
            if (profesional == null)
            {
                return Ok(new { puedePostularEmpleos = false, planActual = "Free", message = "No tenés perfil de profesional" });
            }

            var planActual = ObtenerPlanActualDelProfesional(profesional);
            var puede = PlanLimitsHelper.TieneAccesoEmpleos(planActual);
            return Ok(new { puedePostularEmpleos = puede, planActual });
        }

        [HttpGet("categorias")]
        public async Task<IActionResult> ObtenerCategorias([FromQuery] string? tipo = null)
        {
            try
            {
                // Cargar tipos de servicio usando LINQ con Entity Framework (la tabla existe y tiene datos)
                var tiposServicio = await _context.TipoServicios
                    .AsNoTracking()
                    .Where(ts => ts.Activo)
                    .ToListAsync();

                // Cargar categorías activas
                var categoriasQuery = _context.CategoriaServicios
                    .AsNoTracking()
                    .Where(c => c.Activo);

                // Filtrar por tipo si se especifica
                if (!string.IsNullOrEmpty(tipo))
                {
                    var tipoLower = tipo.ToLower();
                    var tipoServicio = tiposServicio.FirstOrDefault(ts => ts.Nombre.ToLower() == tipoLower);
                    
                    if (tipoServicio != null)
                    {
                        categoriasQuery = categoriasQuery.Where(c => c.TipoServicioId == tipoServicio.Id);
                    }
                    // Si no encuentra el tipo, retornar vacío solo si se especificó un tipo
                    else if (!string.IsNullOrEmpty(tipo))
                    {
                        return Ok(new List<object>());
                    }
                }

                var categorias = await categoriasQuery
                    .OrderBy(c => c.TipoServicioId)
                    .ThenBy(c => c.Nombre)
                    .ToListAsync();

                // Crear diccionario para mapeo rápido (si hay tipos, usarlos; si no, usar valores por defecto)
                var tipoMapping = tiposServicio.Any() 
                    ? tiposServicio.ToDictionary(ts => ts.Id, ts => ts.Nombre)
                    : new Dictionary<int, string> { { 1, "Oficiante" }, { 2, "Profesional" } };

                // Mapear a DTO
                var resultado = categorias
                    .Select(c => new
                    {
                        id = c.Id,
                        nombre = c.Nombre,
                        descripcion = c.Descripcion,
                        tipo = tipoMapping.ContainsKey(c.TipoServicioId) ? tipoMapping[c.TipoServicioId] : "Sin tipo",
                        icono = c.Icono,
                        activo = c.Activo,
                        fechaCreacion = c.FechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    })
                    .OrderBy(c => c.tipo)
                    .ThenBy(c => c.nombre)
                    .ToList();

                return Ok(resultado);
            }
            catch (Microsoft.Data.SqlClient.SqlException sqlEx)
            {
                return StatusCode(500, new
                {
                    message = "Error de base de datos al obtener categorías",
                    error = sqlEx.Message,
                    sqlErrorNumber = sqlEx.Number,
                    hint = sqlEx.Message.Contains("Invalid object name") 
                        ? "La tabla TipoServicios no existe o no es accesible. Ejecuta el script: scripts/VerificarYRecrearTipoServicios.sql"
                        : "Error de SQL Server. Verifica la conexión y los permisos."
                });
            }
            catch (Exception ex)
            {
                // Log del error completo para debugging
                var innerException = ex.InnerException?.Message ?? string.Empty;
                var connectionInfo = "";
                try
                {
                    var conn = _context.Database.GetDbConnection();
                    connectionInfo = $"Base de datos: {conn.Database}, Servidor: {conn.DataSource}, Estado: {conn.State}";
                }
                catch { }
                
                return StatusCode(500, new
                {
                    message = "Error al obtener categorías",
                    error = ex.Message,
                    innerError = innerException,
                    exceptionType = ex.GetType().Name,
                    connectionInfo = connectionInfo,
                    stackTrace = _environment.IsDevelopment() ? ex.StackTrace : null
                });
            }
        }

        [HttpGet("destacados")]
        public async Task<IActionResult> ObtenerDestacados([FromQuery] string? tipo = null)
        {
            try
            {
                // Obtener el ID del tipo de servicio si se especifica
                int? tipoServicioId = null;
                if (!string.IsNullOrWhiteSpace(tipo))
                {
                    var tipoServicio = await _context.TipoServicios
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ts => ts.Nombre.ToLower() == tipo.ToLower() && ts.Activo);
                    
                    if (tipoServicio != null)
                    {
                        tipoServicioId = tipoServicio.Id;
                    }
                }
                
                // Primero verificar si hay profesionales
                var count = await _context.Profesionales.CountAsync(p => p.Activo && p.Disponible);
                
                if (count == 0)
                {
                    return Ok(new
                    {
                        success = true,
                        data = new List<object>()
                    });
                }

                var profesionalesQuery = _context.Profesionales
                    .Include(p => p.Usuario)
                    .Include(p => p.ProfesionalCategorias)
                        .ThenInclude(pc => pc.CategoriaServicio)
                    .Include(p => p.Suscripciones)
                    .Include(p => p.Resenas)
                        .ThenInclude(r => r.Usuario)
                    .Where(p => p.Activo && p.Disponible && p.Usuario != null);

                // Filtrar por tipo de servicio si se especifica
                if (tipoServicioId.HasValue)
                {
                    profesionalesQuery = profesionalesQuery.Where(p => 
                        p.ProfesionalCategorias.Any(pc => 
                            pc.Activo && 
                            pc.CategoriaServicio != null && 
                            pc.CategoriaServicio.TipoServicioId == tipoServicioId.Value));
                }

                // Función para obtener el nivel del plan (mayor número = mejor plan)
                int ObtenerNivelPlan(Profesional p)
                {
                    var suscripcionActiva = p.Suscripciones
                        .FirstOrDefault(s => s.Estado == "Activa" && s.FechaVencimiento > DateTime.UtcNow);
                    
                    if (suscripcionActiva == null)
                        return 0; // Sin plan
                    
                    return suscripcionActiva.TipoSuscripcion.ToLower() switch
                    {
                        "pro" => 3,
                        "premium" => 2,
                        "basica" => 1,
                        _ => 0
                    };
                }

                var profesionales = await profesionalesQuery.ToListAsync();

                // Ordenar por plan, calificación y reseñas
                var profesionalesOrdenados = profesionales
                    .OrderByDescending(p => ObtenerNivelPlan(p))
                    .ThenByDescending(p => p.CalificacionPromedio)
                    .ThenByDescending(p => p.TotalResenas)
                    .Take(6)
                    .ToList();

                var resultado = profesionalesOrdenados
                    .Where(p => p.Usuario != null)
                    .Select(p => new
                    {
                        id = p.Id,
                        usuarioId = p.UsuarioId,
                        descripcion = p.Descripcion ?? string.Empty,
                        fotoPerfil = p.FotoPerfil,
                        calificacionPromedio = p.CalificacionPromedio,
                        totalResenas = p.TotalResenas,
                        disponible = p.Disponible,
                        fechaRegistro = p.FechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        activo = p.Activo,
                        usuario = p.Usuario != null ? new
                        {
                            id = p.Usuario.Id,
                            nombre = p.Usuario.Nombre ?? string.Empty,
                            apellido = p.Usuario.Apellido ?? string.Empty,
                            email = p.Usuario.Email ?? string.Empty,
                            fechaRegistro = p.Usuario.FechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                            esProfesional = false,
                            activo = p.Usuario.Activo
                        } : null,
                        oficios = p.ProfesionalCategorias
                            .Where(pc => pc.Activo && pc.CategoriaServicio != null)
                            .Select(pc => new
                            {
                                nombre = pc.CategoriaServicio!.Nombre ?? string.Empty
                            })
                            .ToList(),
                        certificados = new List<object>(),
                        resenas = p.Resenas
                            .Where(r => r.Activa)
                            .OrderByDescending(r => r.FechaCreacion)
                            .Take(2)
                            .Select(r => new
                            {
                                id = r.Id,
                                calificacion = r.Calificacion,
                                comentario = r.Comentario ?? string.Empty,
                                fechaCreacion = r.FechaCreacion.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                                nombreAutor = r.Usuario != null
                                    ? (r.Usuario.Nombre ?? "") + " " + (r.Usuario.Apellido ?? "").Trim()
                                    : (r.AutorNombre ?? "Anónimo")
                            })
                            .ToList()
                    })
                    .Where(p => p.usuario != null)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    data = resultado
                });
            }
            catch (Exception ex)
            {
                // Log del error completo para debugging
                var innerException = ex.InnerException?.Message ?? string.Empty;
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener profesionales destacados",
                    error = ex.Message,
                    innerError = innerException,
                    stackTrace = _environment.IsDevelopment() ? ex.StackTrace : null
                });
            }
        }

        /// <summary>
        /// Obtiene los perfiles agregados más recientemente (por FechaRegistro), para mostrar en "Perfiles agregados recientemente".
        /// </summary>
        [HttpGet("recientes")]
        public async Task<IActionResult> ObtenerRecientes([FromQuery] int limite = 9)
        {
            try
            {
                var take = Math.Clamp(limite, 1, 50);
                var profesionales = await _context.Profesionales
                    .Include(p => p.Usuario)
                    .Include(p => p.ProfesionalCategorias)
                        .ThenInclude(pc => pc.CategoriaServicio)
                    .Where(p => p.Activo && p.Disponible && p.Usuario != null)
                    .OrderByDescending(p => p.FechaRegistro)
                    .Take(take)
                    .ToListAsync();

                var resultado = profesionales
                    .Select(p => new
                    {
                        id = p.Id,
                        usuarioId = p.UsuarioId,
                        descripcion = p.Descripcion ?? string.Empty,
                        fotoPerfil = p.FotoPerfil,
                        calificacionPromedio = p.CalificacionPromedio,
                        totalResenas = p.TotalResenas,
                        disponible = p.Disponible,
                        fechaRegistro = p.FechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        activo = p.Activo,
                        usuario = p.Usuario != null ? new
                        {
                            id = p.Usuario.Id,
                            nombre = p.Usuario.Nombre ?? string.Empty,
                            apellido = p.Usuario.Apellido ?? string.Empty,
                            email = p.Usuario.Email ?? string.Empty,
                            ciudad = p.Usuario.Ciudad,
                            provincia = p.Usuario.Provincia,
                            fechaRegistro = p.Usuario.FechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                            activo = p.Usuario.Activo
                        } : null,
                        oficios = p.ProfesionalCategorias
                            .Where(pc => pc.Activo && pc.CategoriaServicio != null)
                            .Select(pc => new { nombre = pc.CategoriaServicio!.Nombre ?? string.Empty })
                            .ToList()
                    })
                    .ToList();

                return Ok(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al obtener perfiles recientes", error = ex.Message });
            }
        }

        /// <summary>
        /// Registra un contacto (clic en "Contactar") hacia el profesional. Usado para conteo y planes por contacto.
        /// No requiere autenticación. Respeta límites del plan (Free: 2/mes, Básica: 15/mes, Premium: ilimitado).
        /// </summary>
        [HttpPost("{id}/registrar-contacto")]
        public async Task<IActionResult> RegistrarContacto(int id, [FromBody] RegistrarContactoRequest? request = null)
        {
            var profesional = await _context.Profesionales
                .Include(p => p.Suscripciones)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (profesional == null)
                return NotFound(new { message = "Profesional no encontrado" });

            var planActual = ObtenerPlanActualDelProfesional(profesional);
            var limite = PlanLimitsHelper.GetLimiteContactosMensuales(planActual);
            var inicioMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var finMes = inicioMes.AddMonths(1);
            var contactosEsteMes = await _context.ContactosProfesional
                .CountAsync(c => c.ProfesionalId == id && c.FechaHora >= inicioMes && c.FechaHora < finMes);
            if (contactosEsteMes >= limite)
            {
                return StatusCode(403, new
                {
                    message = "Este profesional alcanzó el límite de contactos de su plan para este mes. Podés intentar más adelante o que actualice su plan.",
                    code = "LIMITE_CONTACTOS",
                    limite,
                    usados = contactosEsteMes
                });
            }

            int? usuarioId = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var uid))
                usuarioId = uid;

            var tipo = request?.TipoContacto?.Trim();
            if (!string.IsNullOrEmpty(tipo) && tipo.Length > 20)
                tipo = tipo.Substring(0, 20);

            _context.ContactosProfesional.Add(new ContactoProfesional
            {
                ProfesionalId = id,
                UsuarioId = usuarioId,
                TipoContacto = tipo ?? "WhatsApp",
                FechaHora = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Contacto registrado" });
        }

        /// <summary>
        /// Devuelve la cantidad de contactos recibidos por el profesional en un período (para planes y panel).
        /// </summary>
        [HttpGet("{id}/contactos/cantidad")]
        public async Task<IActionResult> ObtenerCantidadContactos(int id,
            [FromQuery] DateTime? desde = null,
            [FromQuery] DateTime? hasta = null)
        {
            var profesional = await _context.Profesionales.FindAsync(id);
            if (profesional == null)
                return NotFound(new { message = "Profesional no encontrado" });

            var query = _context.ContactosProfesional.Where(c => c.ProfesionalId == id);
            if (desde.HasValue)
                query = query.Where(c => c.FechaHora >= desde.Value);
            if (hasta.HasValue)
            {
                var fin = hasta.Value.Date.AddDays(1);
                query = query.Where(c => c.FechaHora < fin);
            }
            var total = await query.CountAsync();
            return Ok(new { success = true, total });
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarProfesionales(
            [FromQuery] string? termino = null,
            [FromQuery] int? oficioId = null,
            [FromQuery] int? categoriaId = null,
            [FromQuery] string? oficio = null, // Nombre del oficio/categoría
            [FromQuery] string? zona = null, // Zona/ciudad de búsqueda
            [FromQuery] decimal? precioMin = null,
            [FromQuery] decimal? precioMax = null,
            [FromQuery] string? localidad = null,
            [FromQuery] string? provincia = null)
        {
            try
            {
                var profesionales = await _context.Profesionales
                    .Include(p => p.Usuario)
                    .Include(p => p.ProfesionalCategorias)
                        .ThenInclude(pc => pc.CategoriaServicio)
                    .Include(p => p.ZonasCobertura)
                    .Include(p => p.Suscripciones)
                    .Where(p => p.Activo && p.Disponible)
                    .ToListAsync();

                // Filtrar por término de búsqueda
                if (!string.IsNullOrWhiteSpace(termino))
                {
                    profesionales = profesionales.Where(p => 
                        (p.Descripcion != null && p.Descripcion.Contains(termino, StringComparison.OrdinalIgnoreCase)) ||
                        (p.Usuario.Nombre != null && p.Usuario.Nombre.Contains(termino, StringComparison.OrdinalIgnoreCase)) ||
                        (p.Usuario.Apellido != null && p.Usuario.Apellido.Contains(termino, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                // Filtrar por oficio
                if (oficioId.HasValue)
                {
                    profesionales = profesionales.Where(p => 
                        p.ProfesionalOficios.Any(po => po.OficioId == oficioId.Value && po.Activo)
                    ).ToList();
                }

                // Filtrar por categoría (por ID)
                if (categoriaId.HasValue)
                {
                    profesionales = profesionales.Where(p => 
                        p.ProfesionalCategorias.Any(pc => pc.CategoriaServicioId == categoriaId.Value && pc.Activo)
                    ).ToList();
                }

                // Filtrar por nombre de oficio/categoría (por nombre - búsqueda flexible)
                if (!string.IsNullOrWhiteSpace(oficio))
                {
                    var oficioNormalizado = oficio.Trim();
                    profesionales = profesionales.Where(p => 
                        p.ProfesionalCategorias.Any(pc => pc.Activo && 
                            pc.CategoriaServicio != null && 
                            pc.CategoriaServicio.Nombre != null &&
                            (pc.CategoriaServicio.Nombre.Equals(oficioNormalizado, StringComparison.OrdinalIgnoreCase) ||
                             pc.CategoriaServicio.Nombre.Contains(oficioNormalizado, StringComparison.OrdinalIgnoreCase) ||
                             oficioNormalizado.Contains(pc.CategoriaServicio.Nombre, StringComparison.OrdinalIgnoreCase)))
                    ).ToList();
                }

                // Filtrar por zona/ciudad (busca en zonas de cobertura Y en la ciudad del usuario)
                if (!string.IsNullOrWhiteSpace(zona))
                {
                    var zonaNormalizada = zona.Trim();
                    // Extraer solo el nombre de la ciudad si viene en formato "Ciudad, Departamento, Provincia"
                    var partesCiudad = zonaNormalizada.Split(',');
                    var nombreCiudad = partesCiudad.Length > 0 ? partesCiudad[0].Trim() : zonaNormalizada;
                    
                    profesionales = profesionales.Where(p => 
                    {
                        // Buscar en zonas de cobertura
                        bool coincideEnZonas = p.ZonasCobertura.Any(z => z.Activo && 
                            ((!string.IsNullOrWhiteSpace(z.Localidad) && 
                              (z.Localidad.Contains(zonaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                               z.Localidad.Contains(nombreCiudad, StringComparison.OrdinalIgnoreCase) ||
                               nombreCiudad.Contains(z.Localidad, StringComparison.OrdinalIgnoreCase))) ||
                             (!string.IsNullOrWhiteSpace(z.Departamento) && 
                              (z.Departamento.Contains(zonaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                               z.Departamento.Contains(nombreCiudad, StringComparison.OrdinalIgnoreCase))) ||
                             (!string.IsNullOrWhiteSpace(z.Provincia) && 
                              z.Provincia.Contains(zonaNormalizada, StringComparison.OrdinalIgnoreCase))));
                        
                        // También buscar en la ciudad del usuario como fallback
                        bool coincideEnCiudadUsuario = p.Usuario != null && 
                            !string.IsNullOrWhiteSpace(p.Usuario.Ciudad) && 
                            (p.Usuario.Ciudad.Contains(zonaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                             p.Usuario.Ciudad.Contains(nombreCiudad, StringComparison.OrdinalIgnoreCase));
                        
                        bool coincideEnProvinciaUsuario = p.Usuario != null && 
                            !string.IsNullOrWhiteSpace(p.Usuario.Provincia) && 
                            p.Usuario.Provincia.Contains(zonaNormalizada, StringComparison.OrdinalIgnoreCase);
                        
                        return coincideEnZonas || coincideEnCiudadUsuario || coincideEnProvinciaUsuario;
                    }).ToList();
                }

                // Filtrar por localidad o provincia (parámetros originales)
                if (!string.IsNullOrWhiteSpace(localidad) || !string.IsNullOrWhiteSpace(provincia))
                {
                    profesionales = profesionales.Where(p => 
                        p.ZonasCobertura.Any(z => z.Activo && 
                            (string.IsNullOrWhiteSpace(localidad) || z.Localidad.Contains(localidad, StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrWhiteSpace(provincia) || z.Provincia.Contains(provincia, StringComparison.OrdinalIgnoreCase)))
                    ).ToList();
                }

                // Función para obtener el nivel del plan (mayor número = mejor plan)
                int ObtenerNivelPlan(Profesional p)
                {
                    var suscripcionActiva = p.Suscripciones
                        .FirstOrDefault(s => s.Estado == "Activa" && s.FechaVencimiento > DateTime.UtcNow);
                    
                    if (suscripcionActiva == null)
                        return 0; // Sin plan
                    
                    return suscripcionActiva.TipoSuscripcion.ToLower() switch
                    {
                        "pro" => 3,
                        "premium" => 2,
                        "basica" => 1,
                        _ => 0
                    };
                }

                // Ordenar profesionales antes de proyectar
                var profesionalesOrdenados = profesionales
                    .Where(p => p.Usuario != null)
                    .OrderByDescending(p => ObtenerNivelPlan(p)) // Primero por plan (mejor plan primero)
                    .ThenByDescending(p => p.CalificacionPromedio) // Luego por calificación
                    .ThenByDescending(p => p.TotalResenas) // Finalmente por cantidad de reseñas
                    .ToList();

                var resultado = profesionalesOrdenados
                    .Select(p => new
                    {
                        id = p.Id,
                        usuarioId = p.UsuarioId,
                        descripcion = p.Descripcion ?? string.Empty,
                        fotoPerfil = p.FotoPerfil,
                        calificacionPromedio = p.CalificacionPromedio,
                        totalResenas = p.TotalResenas,
                        disponible = p.Disponible,
                        fechaRegistro = p.FechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        activo = p.Activo,
                        planActual = p.Suscripciones
                            .Where(s => s.Estado == "Activa" && s.FechaVencimiento > DateTime.UtcNow)
                            .OrderByDescending(s => s.FechaVencimiento)
                            .Select(s => s.TipoSuscripcion)
                            .FirstOrDefault() ?? "Sin plan",
                        usuario = p.Usuario != null ? new
                        {
                            id = p.Usuario.Id,
                            nombre = p.Usuario.Nombre ?? string.Empty,
                            apellido = p.Usuario.Apellido ?? string.Empty,
                            email = p.Usuario.Email ?? string.Empty,
                            fechaRegistro = p.Usuario.FechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                            esProfesional = false,
                            activo = p.Usuario.Activo
                        } : null,
                        oficios = p.ProfesionalCategorias
                            .Where(pc => pc.Activo && pc.CategoriaServicio != null)
                            .Select(pc => new
                            {
                                nombre = pc.CategoriaServicio!.Nombre ?? string.Empty
                            })
                            .ToList(),
                        zonasCobertura = p.ZonasCobertura
                            .Where(z => z.Activo)
                            .Select(z => new
                            {
                                localidad = z.Localidad,
                                provincia = z.Provincia,
                                departamento = z.Departamento,
                                latitud = z.Latitud,
                                longitud = z.Longitud
                            })
                            .ToList()
                    })
                    .Where(p => p.usuario != null)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    data = resultado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al buscar profesionales",
                    error = ex.Message
                });
            }
        }

        private async Task<string?> GuardarFotoPerfilAsync(int profesionalId, string dataUrl)
        {
            if (string.IsNullOrWhiteSpace(dataUrl) || !dataUrl.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                return null;

            var match = System.Text.RegularExpressions.Regex.Match(dataUrl, @"^data:image/(\w+);base64,(.+)$");
            if (!match.Success) return null;

            var extension = match.Groups[1].Value.ToLowerInvariant() switch
            {
                "jpeg" or "jpg" => ".jpg",
                "png" => ".png",
                "gif" => ".gif",
                "webp" => ".webp",
                _ => ".jpg"
            };
            byte[] bytes;
            try { bytes = Convert.FromBase64String(match.Groups[2].Value); }
            catch { return null; }

            var webRoot = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadsDir = Path.Combine(webRoot, "uploads", "perfiles");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = $"{profesionalId}_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadsDir, fileName);
            await System.IO.File.WriteAllBytesAsync(filePath, bytes);
            return $"/uploads/perfiles/{fileName}";
        }

        private async Task GuardarImagenesTrabajoAsync(int profesionalId, List<string> imagenesBase64)
        {
            var webRoot = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadsDir = Path.Combine(webRoot, "uploads", "trabajos");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            int orden = 0;
            foreach (var dataUrl in imagenesBase64)
            {
                if (string.IsNullOrWhiteSpace(dataUrl) || !dataUrl.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                    continue;

                var match = System.Text.RegularExpressions.Regex.Match(dataUrl, @"^data:image/(\w+);base64,(.+)$");
                if (!match.Success)
                    continue;

                var extension = match.Groups[1].Value.ToLowerInvariant() switch
                {
                    "jpeg" or "jpg" => ".jpg",
                    "png" => ".png",
                    "gif" => ".gif",
                    "webp" => ".webp",
                    _ => ".jpg"
                };
                var base64 = match.Groups[2].Value;
                byte[] bytes;
                try
                {
                    bytes = Convert.FromBase64String(base64);
                }
                catch
                {
                    continue;
                }

                var fileName = $"{profesionalId}_{Guid.NewGuid():N}{extension}";
                var filePath = Path.Combine(uploadsDir, fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, bytes);

                var relativeUrl = $"/uploads/trabajos/{fileName}";
                _context.ImagenesTrabajo.Add(new ImagenTrabajo
                {
                    ProfesionalId = profesionalId,
                    UrlImagen = relativeUrl,
                    Orden = orden++,
                    Activo = true
                });
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>Obtiene el nombre del plan actual (suscripción activa y no vencida) o "Free".</summary>
        private static string ObtenerPlanActualDelProfesional(Profesional p)
        {
            var activa = p.Suscripciones?.FirstOrDefault(s => s.Estado == "Activa" && s.FechaVencimiento > DateTime.UtcNow);
            return activa?.TipoSuscripcion ?? "Free";
        }
    }

    public class ValidarImagenesRequest
    {
        public List<string>? ImagenesBase64 { get; set; }
    }
}
