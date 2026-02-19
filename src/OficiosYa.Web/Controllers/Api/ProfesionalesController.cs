using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OficiosYa.Application.DTOs.Request;
using OficiosYa.Application.UseCases.Profesionales.RegistrarProfesional;
using OficiosYa.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Data.Common;
using OficiosYa.Domain.Entities;

namespace OficiosYa.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesionalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProfesionalesController(IMediator mediator, ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _mediator = mediator;
            _context = context;
            _environment = environment;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarProfesional([FromBody] RegistrarProfesionalRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new RegistrarProfesionalCommand { Request = request };
                var profesional = await _mediator.Send(command);
                
                if (profesional == null)
                {
                    return Conflict(new { message = "El email ya está registrado" });
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

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerProfesional(int id)
        {
            var profesional = await _context.Profesionales
                .Include(p => p.Usuario)
                .Include(p => p.ProfesionalCategorias)
                    .ThenInclude(pc => pc.CategoriaServicio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profesional == null)
            {
                return NotFound(new { message = "Profesional no encontrado" });
            }

            return Ok(new
            {
                id = profesional.Id,
                nombre = profesional.Usuario.Nombre,
                apellido = profesional.Usuario.Apellido,
                email = profesional.Usuario.Email,
                descripcion = profesional.Descripcion,
                calificacionPromedio = profesional.CalificacionPromedio,
                totalResenas = profesional.TotalResenas,
                disponible = profesional.Disponible,
                oficios = profesional.ProfesionalCategorias
                    .Where(pc => pc.Activo)
                    .Select(pc => pc.CategoriaServicio.Nombre)
                    .ToList()
            });
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
                        resenas = new List<object>()
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
    }

}
