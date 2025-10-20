using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosYa.Web.Models
{
    public class Suscripcion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoSuscripcion { get; set; } = string.Empty; // "Basica", "Premium", "Pro"

        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaVencimiento { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } = "Activa"; // "Activa", "Vencida", "Cancelada"

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Navegación
        [ForeignKey("ProfesionalId")]
        public virtual Profesional Profesional { get; set; } = null!;
    }
}
