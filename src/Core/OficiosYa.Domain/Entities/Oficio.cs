using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class Oficio
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [StringLength(200)]
        public string? Icono { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public virtual ICollection<ProfesionalOficio> ProfesionalOficios { get; set; } = new List<ProfesionalOficio>();

        // Métodos de dominio
        public void Desactivar()
        {
            Activo = false;
        }

        public void Activar()
        {
            Activo = true;
        }
    }
}
