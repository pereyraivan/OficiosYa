using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Application.DTOs.Request
{
    public class RestablecerContrasenaRequest
    {
        [Required(ErrorMessage = "El token es obligatorio")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NuevaContrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes confirmar la contraseña")]
        [Compare(nameof(NuevaContrasena), ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; } = string.Empty;
    }
}
