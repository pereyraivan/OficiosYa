using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Persistence.Context;
using OficiosYa.Web.Contracts;

namespace OficiosYa.Web.Controllers.Api.Admin;

[ApiController]
[Route("api/admin/[controller]")]
public class SuscripcionesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IUsuarioRepository _usuarioRepository;

    public SuscripcionesController(ApplicationDbContext context, IUsuarioRepository usuarioRepository)
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

    /// <summary>Suscripciones pendientes de confirmación de pago (transferencia).</summary>
    [HttpGet("pendientes")]
    public async Task<IActionResult> ObtenerPendientes(CancellationToken cancellationToken)
    {
        if (!await EsAdminAsync())
            return Unauthorized(new { message = "No autorizado" });

        var pendientes = await _context.Suscripciones
            .AsNoTracking()
            .Where(s => s.Estado == "Pendiente" && !s.PagoConfirmado)
            .Include(s => s.Profesional)
                .ThenInclude(p => p.Usuario)
            .Include(s => s.PlanSuscripcion)
            .OrderByDescending(s => s.FechaRegistro)
            .Select(s => new AdminSuscripcionPendienteDto
            {
                Id = s.Id,
                ProfesionalId = s.ProfesionalId,
                ProfesionalNombre = s.Profesional.Usuario != null
                    ? (s.Profesional.Usuario.Nombre + " " + s.Profesional.Usuario.Apellido).Trim()
                    : string.Empty,
                ProfesionalEmail = s.Profesional.Usuario != null ? s.Profesional.Usuario.Email : string.Empty,
                ProfesionalTelefono = s.Profesional.Usuario != null ? s.Profesional.Usuario.Telefono : null,
                ProfesionalDocumento = s.Profesional.Usuario != null ? s.Profesional.Usuario.Documento : null,
                PlanSuscripcionId = s.PlanSuscripcionId,
                PlanNombre = s.PlanSuscripcion != null
                    ? s.PlanSuscripcion.Nombre
                    : s.TipoSuscripcion,
                Precio = s.Precio,
                CodigoPago = s.CodigoPago,
                MetodoPago = s.MetodoPago,
                Estado = s.Estado,
                FechaRegistro = s.FechaRegistro,
                FechaInicio = s.FechaInicio,
                FechaVencimiento = s.FechaVencimiento,
                MesesDuracion = Math.Max(1,
                    ((s.FechaVencimiento.Year - s.FechaInicio.Year) * 12)
                    + s.FechaVencimiento.Month - s.FechaInicio.Month)
            })
            .ToListAsync(cancellationToken);

        return Ok(pendientes);
    }

    /// <summary>Confirma el pago y activa la suscripción desde la fecha actual.</summary>
    [HttpPost("{id:int}/confirmar")]
    public async Task<IActionResult> ConfirmarPago(int id, CancellationToken cancellationToken)
    {
        if (!await EsAdminAsync())
            return Unauthorized(new { message = "No autorizado" });

        var suscripcion = await _context.Suscripciones
            .Include(s => s.Profesional)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (suscripcion == null)
            return NotFound(new { message = "Suscripción no encontrada" });

        if (suscripcion.PagoConfirmado || suscripcion.Estado == "Activa")
            return BadRequest(new { message = "Esta suscripción ya fue confirmada." });

        if (suscripcion.Estado == "Cancelada")
            return BadRequest(new { message = "Esta suscripción fue cancelada y no puede confirmarse." });

        var meses = CalcularMesesDuracion(suscripcion.FechaInicio, suscripcion.FechaVencimiento);
        var ahora = DateTime.UtcNow;

        // Desactivar otras suscripciones activas del mismo profesional
        var otrasActivas = await _context.Suscripciones
            .Where(s => s.ProfesionalId == suscripcion.ProfesionalId
                        && s.Id != suscripcion.Id
                        && s.Estado == "Activa")
            .ToListAsync(cancellationToken);

        foreach (var otra in otrasActivas)
        {
            otra.Estado = "Cancelada";
        }

        suscripcion.PagoConfirmado = true;
        suscripcion.Estado = "Activa";
        suscripcion.FechaPago = ahora;
        suscripcion.FechaInicio = ahora;
        suscripcion.FechaVencimiento = ahora.AddMonths(meses);
        suscripcion.Activar();

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new AdminConfirmarSuscripcionResponse
        {
            Id = suscripcion.Id,
            Estado = suscripcion.Estado,
            PagoConfirmado = suscripcion.PagoConfirmado,
            FechaPago = suscripcion.FechaPago,
            FechaInicio = suscripcion.FechaInicio,
            FechaVencimiento = suscripcion.FechaVencimiento,
            Mensaje = $"Suscripción activada hasta {suscripcion.FechaVencimiento:dd/MM/yyyy}."
        });
    }

    /// <summary>Rechaza o cancela una suscripción pendiente (pago no acreditado).</summary>
    [HttpPost("{id:int}/rechazar")]
    public async Task<IActionResult> RechazarPago(int id, CancellationToken cancellationToken)
    {
        if (!await EsAdminAsync())
            return Unauthorized(new { message = "No autorizado" });

        var suscripcion = await _context.Suscripciones
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (suscripcion == null)
            return NotFound(new { message = "Suscripción no encontrada" });

        if (suscripcion.PagoConfirmado)
            return BadRequest(new { message = "No se puede rechazar una suscripción ya confirmada." });

        suscripcion.Estado = "Cancelada";
        suscripcion.PagoConfirmado = false;
        suscripcion.Cancelar();

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { message = "Suscripción marcada como cancelada.", id = suscripcion.Id });
    }

    private static int CalcularMesesDuracion(DateTime fechaInicio, DateTime fechaVencimiento)
    {
        var meses = ((fechaVencimiento.Year - fechaInicio.Year) * 12)
                    + fechaVencimiento.Month - fechaInicio.Month;
        if (meses <= 0)
            meses = 1;
        return meses;
    }
}
