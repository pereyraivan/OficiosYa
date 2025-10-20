using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosYa.Web.Models
{
    public class ProfesionalOficio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        public int OficioId { get; set; }

        [StringLength(500)]
        public string? DescripcionEspecializada { get; set; }

        public decimal PrecioHora { get; set; }

        public int AnosExperiencia { get; set; } = 0;

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Navegación
        [ForeignKey("ProfesionalId")]
        public virtual Profesional Profesional { get; set; } = null!;

        [ForeignKey("OficioId")]
        public virtual Oficio Oficio { get; set; } = null!;
    }
}
