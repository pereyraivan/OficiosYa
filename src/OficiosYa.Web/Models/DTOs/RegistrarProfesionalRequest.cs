using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Web.Models.DTOs
{
    public class RegistrarProfesionalRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

        [StringLength(100)]
        public string? Ciudad { get; set; }

        [StringLength(100)]
        public string? Provincia { get; set; }

        [StringLength(10)]
        public string? CodigoPostal { get; set; }

        // Datos específicos del profesional
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        public List<string> Oficios { get; set; } = new List<string>();

        [StringLength(200)]
        public string? FotoPerfil { get; set; }
    }
}
