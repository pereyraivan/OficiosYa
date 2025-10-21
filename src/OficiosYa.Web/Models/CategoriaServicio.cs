using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Web.Models
{
    public class CategoriaServicio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty; // "Oficiante" o "Profesional"

        [StringLength(10)]
        public string? Icono { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public virtual ICollection<ProfesionalCategoria> ProfesionalCategorias { get; set; } = new List<ProfesionalCategoria>();
    }
}
