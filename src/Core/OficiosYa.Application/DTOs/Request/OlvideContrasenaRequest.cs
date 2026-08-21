using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Application.DTOs.Request
{
    public class OlvideContrasenaRequest
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;
    }
}
