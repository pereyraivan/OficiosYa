using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosYa.Web.Models
{
    public class ProfesionalCategoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        public int CategoriaServicioId { get; set; }

        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        // Navegación
        [ForeignKey("ProfesionalId")]
        public virtual Profesional Profesional { get; set; } = null!;

        [ForeignKey("CategoriaServicioId")]
        public virtual CategoriaServicio CategoriaServicio { get; set; } = null!;
    }
}
