using System.Security.Cryptography;
using AutoMapper;
using BCrypt.Net;
using MediatR;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Application.Services;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Profesionales.RegistrarProfesional
{
    public class RegistrarProfesionalHandler : IRequestHandler<RegistrarProfesionalCommand, ProfesionalResponse?>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IRepository<Suscripcion> _suscripcionRepository;
        private readonly IPlanSuscripcionService _planSuscripcionService;
        private readonly IMapper _mapper;

        public RegistrarProfesionalHandler(
            IUsuarioRepository usuarioRepository,
            IProfesionalRepository profesionalRepository,
            IRepository<Suscripcion> suscripcionRepository,
            IPlanSuscripcionService planSuscripcionService,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _profesionalRepository = profesionalRepository;
            _suscripcionRepository = suscripcionRepository;
            _planSuscripcionService = planSuscripcionService;
            _mapper = mapper;
        }

        public async Task<ProfesionalResponse?> Handle(RegistrarProfesionalCommand request, CancellationToken cancellationToken)
        {
            // Verificar si el email ya existe
            if (await _usuarioRepository.EmailExistsAsync(request.Request.Email))
            {
                return null;
            }

            // Validar que se hayan seleccionado oficios/categorías
            if (request.Request.Oficios == null || !request.Request.Oficios.Any())
            {
                throw new ArgumentException("Debe seleccionar al menos un oficio o categoría");
            }

            // Crear nuevo usuario
            if (string.IsNullOrWhiteSpace(request.Request.Password))
            {
                throw new ArgumentException("La contraseña es obligatoria");
            }

            var usuario = new Usuario
            {
                Nombre = request.Request.Nombre,
                Apellido = request.Request.Apellido,
                Email = request.Request.Email.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.Password),
                Telefono = request.Request.Telefono,
                Direccion = request.Request.Direccion,
                Ciudad = request.Request.Ciudad,
                Provincia = request.Request.Provincia,
                CodigoPostal = request.Request.CodigoPostal,
                FechaRegistro = DateTime.UtcNow,
                EmailVerificado = false,
                Activo = true
            };

            Usuario usuarioCreado;
            try
            {
                usuarioCreado = await _usuarioRepository.AddAsync(usuario);
            }
            catch (Exception ex)
            {
                // No referenciamos EF/SqlClient directamente en este proyecto: inspeccionamos la excepción base por nombre
                var baseEx = ex.GetBaseException();
                var typeName = baseEx.GetType().FullName ?? string.Empty;
                if (typeName == "Microsoft.Data.SqlClient.SqlException" || typeName == "System.Data.SqlClient.SqlException")
                {
                    dynamic sqlEx = baseEx;
                    int number = 0;
                    try { number = (int)sqlEx.Number; } catch { number = 0; }
                    string msg = sqlEx.Message ?? baseEx.Message;

                    // Unique constraint violation (email already exists)
                    if (number == 2627 || number == 2601)
                    {
                        return null; // Controller will map null to Conflict (email exists)
                    }

                    // NULL insert into column (schema out of sync)
                    if (msg.IndexOf("Cannot insert the value NULL into column", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        throw new InvalidOperationException($"Error al guardar usuario: columna obligatoria faltante. Detalle SQL: {msg}", ex);
                    }
                }

                throw; // rethrow if we don't handle it
            }

            // Crear perfil profesional
            var profesional = new Profesional
            {
                UsuarioId = usuarioCreado.Id,
                Descripcion = request.Request.Descripcion,
                CalificacionPromedio = 0,
                TotalResenas = 0,
                Disponible = true,
                Activo = true,
                FechaRegistro = DateTime.UtcNow
            };

            var profesionalCreado = await _profesionalRepository.AddAsync(profesional);

            // Asociar categorías seleccionadas usando el repositorio
            await _profesionalRepository.AsociarCategoriasAsync(profesionalCreado.Id, request.Request.Oficios);

            // Asociar zonas de cobertura si se proporcionaron (plan Free = 1 zona; al registrarse no tiene suscripción activa)
            if (request.Request.ZonasCobertura != null && request.Request.ZonasCobertura.Any())
            {
                const int maxZonasRegistro = 1; // Sin plan activo = Free = 1 zona
                var zonas = request.Request.ZonasCobertura
                    .Take(maxZonasRegistro)
                    .Select(z => new ZonaCobertura
                    {
                        Localidad = z.Localidad,
                        Provincia = z.Provincia,
                        Departamento = z.Departamento,
                        Latitud = z.Latitud,
                        Longitud = z.Longitud,
                        CodigoPostal = z.CodigoPostal
                    }).ToList();

                await _profesionalRepository.AsociarZonasCoberturaAsync(profesionalCreado.Id, zonas);
            }

            // Crear suscripción inicial según el plan elegido en el registro.
            var mesesSuscripcion = request.Request.MesesSuscripcion ?? 1;
            var planSolicitado = (request.Request.PlanSeleccionado ?? "Free").Trim();
            var planSolicitadoNormalizado = planSolicitado.ToLowerInvariant() switch
            {
                "basica" => "Basico",
                "basico" => "Basico",
                _ => planSolicitado
            };

            string? codigoTransferenciaGenerado = null;
            decimal? montoSuscripcionPendiente = null;

            // Free no requiere crear registro de suscripción.
            if (!string.Equals(planSolicitadoNormalizado, "free", StringComparison.OrdinalIgnoreCase))
            {
                var planSeleccionado = await _planSuscripcionService.ObtenerPlanPorNombreAsync(planSolicitadoNormalizado);
                if (planSeleccionado == null)
                {
                    throw new ArgumentException($"El plan '{planSolicitado}' no existe.");
                }

                // PrecioMensual / Trimestral / etc. en BD ya son montos finales por período (no aplicar descuento otra vez).
                var precioSuscripcion = await _planSuscripcionService.CalcularPrecioAsync(planSeleccionado.Nombre, mesesSuscripcion);
                var fechaInicio = DateTime.UtcNow;
                var fechaVencimiento = fechaInicio.AddMonths(mesesSuscripcion);

                var metodoPago = request.Request.MetodoPago?.Trim();
                if (string.IsNullOrEmpty(metodoPago))
                    metodoPago = "transferencia";
                var metodoLower = metodoPago.ToLowerInvariant();
                string? codigoPagoValor = null;
                if (metodoLower == "transferencia")
                {
                    codigoPagoValor = await GenerarCodigoPagoUnicoAsync();
                    codigoTransferenciaGenerado = codigoPagoValor;
                    montoSuscripcionPendiente = precioSuscripcion;
                    metodoPago = "transferencia";
                }
                else if (metodoLower is "rapipago" or "pagosfaciles")
                {
                    codigoPagoValor = request.Request.CodigoPago;
                }

                var suscripcion = new Suscripcion
                {
                    ProfesionalId = profesionalCreado.Id,
                    PlanSuscripcionId = planSeleccionado.Id,
                    TipoSuscripcion = planSeleccionado.Nombre,
                    Precio = precioSuscripcion,
                    FechaInicio = fechaInicio,
                    FechaVencimiento = fechaVencimiento,
                    Estado = "Pendiente", // Pendiente hasta confirmar el pago
                    MetodoPago = metodoPago,
                    CodigoPago = codigoPagoValor,
                    PagoConfirmado = false,
                    FechaRegistro = DateTime.UtcNow
                };

                await _suscripcionRepository.AddAsync(suscripcion);
            }

            // Cargar el profesional con sus categorías para el response
            var profesionalCompleto = await _profesionalRepository.GetWithCategoriasAsync(profesionalCreado.Id);
            
            var response = _mapper.Map<ProfesionalResponse>(profesionalCompleto);
            if (response != null && profesionalCompleto != null)
            {
                response.Nombre = usuarioCreado.Nombre;
                response.Apellido = usuarioCreado.Apellido;
                response.Email = usuarioCreado.Email;
                response.UsuarioId = usuarioCreado.Id;
                response.Oficios = profesionalCompleto.ProfesionalCategorias
                    .Where(pc => pc.Activo)
                    .Select(pc => pc.CategoriaServicio.Nombre)
                    .ToList();
                response.CodigoReferenciaTransferencia = codigoTransferenciaGenerado;
                response.MontoSuscripcionPendienteArs = montoSuscripcionPendiente;
            }

            return response;
        }

        private async Task<string> GenerarCodigoPagoUnicoAsync()
        {
            for (var i = 0; i < 12; i++)
            {
                var codigo = CrearCodigoReferenciaTransferencia();
                var existe = await _suscripcionRepository.AnyAsync(s => s.CodigoPago == codigo);
                if (!existe)
                    return codigo;
            }

            return CrearCodigoReferenciaTransferencia();
        }

        private static string CrearCodigoReferenciaTransferencia()
        {
            Span<byte> bytes = stackalloc byte[6];
            RandomNumberGenerator.Fill(bytes);
            return "OY-" + Convert.ToHexString(bytes);
        }
    }
}

