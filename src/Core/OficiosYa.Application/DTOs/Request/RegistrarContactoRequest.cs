using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Application.DTOs.Request
{
    /// <summary>
    /// Opcional: tipo de canal por el que el usuario contacta (WhatsApp, Email).
    /// </summary>
    public class RegistrarContactoRequest
    {
        [StringLength(20)]
        public string? TipoContacto { get; set; }
    }
}
