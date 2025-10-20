using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosYa.Web.Models
{
    public class Certificado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [StringLength(200)]
        public string? Institucion { get; set; }

        [StringLength(100)]
        public string? NumeroCertificado { get; set; }

        public DateTime? FechaEmision { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        [StringLength(500)]
        public string? UrlImagen { get; set; }

        public bool Verificado { get; set; } = false;

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Navegación
        [ForeignKey("ProfesionalId")]
        public virtual Profesional Profesional { get; set; } = null!;
    }
}
