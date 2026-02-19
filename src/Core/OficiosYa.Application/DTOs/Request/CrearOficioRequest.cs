using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Application.DTOs.Request
{
    public class CrearOficioRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [StringLength(200, ErrorMessage = "El icono no puede exceder 200 caracteres")]
        public string? Icono { get; set; }

        [StringLength(50, ErrorMessage = "El color no puede exceder 50 caracteres")]
        public string? Color { get; set; }
    }
}

