using MediatR;
using OficiosYa.Application.Common;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Application.Services;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;

namespace OficiosYa.Application.UseCases.Profesionales.SolicitarSuscripcion
{
    public class SolicitarSuscripcionHandler : IRequestHandler<SolicitarSuscripcionCommand, SolicitarSuscripcionResponse>
    {
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRepository<Suscripcion> _suscripcionRepository;
        private readonly IPlanSuscripcionService _planSuscripcionService;

        public SolicitarSuscripcionHandler(
            IProfesionalRepository profesionalRepository,
            IUsuarioRepository usuarioRepository,
            IRepository<Suscripcion> suscripcionRepository,
            IPlanSuscripcionService planSuscripcionService)
        {
            _profesionalRepository = profesionalRepository;
            _usuarioRepository = usuarioRepository;
            _suscripcionRepository = suscripcionRepository;
            _planSuscripcionService = planSuscripcionService;
        }

        public async Task<SolicitarSuscripcionResponse> Handle(SolicitarSuscripcionCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var profesional = await _profesionalRepository.GetByUsuarioIdAsync(command.UsuarioId);
            if (profesional == null)
                throw new InvalidOperationException("No tenés un perfil de profesional asociado a esta cuenta.");

            var usuario = await _usuarioRepository.GetByIdAsync(command.UsuarioId);
            var documento = DocumentoIdentidad.Normalizar(usuario?.Documento);
            if (!DocumentoIdentidad.EsValido(documento))
            {
                throw new InvalidOperationException(
                    "Para pagar por transferencia necesitamos tu DNI o CUIT. Completalo en tu perfil o contactá soporte.");
            }

            var planSolicitado = (request.PlanSeleccionado ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(planSolicitado) ||
                planSolicitado.Equals("free", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("El plan Free no requiere contratación.");
            }

            var planSolicitadoNormalizado = planSolicitado.ToLowerInvariant() switch
            {
                "basica" => "Basico",
                "basico" => "Basico",
                _ => planSolicitado
            };

            var meses = request.MesesSuscripcion <= 0 ? 1 : request.MesesSuscripcion;
            if (meses is not (1 or 3 or 6 or 12))
                throw new ArgumentException("Duración inválida. Elegí 1, 3, 6 o 12 meses.");

            var pendienteExistente = await _suscripcionRepository.FirstOrDefaultAsync(s =>
                s.ProfesionalId == profesional.Id &&
                s.Estado == "Pendiente" &&
                !s.PagoConfirmado);

            if (pendienteExistente != null)
            {
                // Preferir documento actual del usuario como referencia de concepto.
                if (string.IsNullOrWhiteSpace(pendienteExistente.CodigoPago) ||
                    pendienteExistente.CodigoPago.StartsWith("OY-", StringComparison.OrdinalIgnoreCase))
                {
                    pendienteExistente.CodigoPago = documento;
                    await _suscripcionRepository.UpdateAsync(pendienteExistente);
                }

                return new SolicitarSuscripcionResponse
                {
                    SuscripcionId = pendienteExistente.Id,
                    CodigoReferenciaTransferencia = pendienteExistente.CodigoPago ?? documento,
                    MontoSuscripcionPendienteArs = pendienteExistente.Precio,
                    PlanNombre = pendienteExistente.TipoSuscripcion,
                    Meses = CalcularMesesDuracion(pendienteExistente.FechaInicio, pendienteExistente.FechaVencimiento),
                    YaExistente = true
                };
            }

            var planSeleccionado = await _planSuscripcionService.ObtenerPlanPorNombreAsync(planSolicitadoNormalizado);
            if (planSeleccionado == null)
                throw new ArgumentException($"El plan '{planSolicitado}' no existe.");

            var precioSuscripcion = await _planSuscripcionService.CalcularPrecioAsync(planSeleccionado.Nombre, meses);
            var fechaInicio = DateTime.UtcNow;
            var fechaVencimiento = fechaInicio.AddMonths(meses);

            var metodoPago = (request.MetodoPago ?? "transferencia").Trim().ToLowerInvariant();
            if (metodoPago != "transferencia")
                throw new ArgumentException("Por ahora solo está disponible el pago por transferencia.");

            var suscripcion = new Suscripcion
            {
                ProfesionalId = profesional.Id,
                PlanSuscripcionId = planSeleccionado.Id,
                TipoSuscripcion = planSeleccionado.Nombre,
                Precio = precioSuscripcion,
                FechaInicio = fechaInicio,
                FechaVencimiento = fechaVencimiento,
                Estado = "Pendiente",
                MetodoPago = "transferencia",
                CodigoPago = documento,
                PagoConfirmado = false,
                FechaRegistro = DateTime.UtcNow
            };

            await _suscripcionRepository.AddAsync(suscripcion);

            return new SolicitarSuscripcionResponse
            {
                SuscripcionId = suscripcion.Id,
                CodigoReferenciaTransferencia = documento,
                MontoSuscripcionPendienteArs = precioSuscripcion,
                PlanNombre = planSeleccionado.Nombre,
                Meses = meses,
                YaExistente = false
            };
        }

        private static int CalcularMesesDuracion(DateTime inicio, DateTime vencimiento)
        {
            var meses = (vencimiento.Year - inicio.Year) * 12 + (vencimiento.Month - inicio.Month);
            return meses <= 0 ? 1 : meses;
        }
    }
}
