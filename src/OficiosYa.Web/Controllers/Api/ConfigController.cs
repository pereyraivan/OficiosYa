using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OficiosYa.Application.Options;

namespace OficiosYa.Web.Controllers.Api;

[ApiController]
[Route("api/config")]
public class ConfigController : ControllerBase
{
    private readonly LanzamientoPromoOptions _promo;

    public ConfigController(IOptions<LanzamientoPromoOptions> promoOptions)
    {
        _promo = promoOptions.Value;
    }

    /// <summary>Estado público de la promo de lanzamiento (registro gratis con mejor plan).</summary>
    [HttpGet("lanzamiento-promo")]
    public IActionResult GetLanzamientoPromo()
    {
        var activa = _promo.EstaActivaAhora();
        return Ok(new
        {
            activa,
            planNombre = activa ? _promo.PlanNombre : null,
            diasGratis = activa ? _promo.DiasGratis : 0,
            fechaFinUtc = _promo.FechaFinUtc,
            mensaje = activa ? _promo.MensajePublico : null
        });
    }
}
