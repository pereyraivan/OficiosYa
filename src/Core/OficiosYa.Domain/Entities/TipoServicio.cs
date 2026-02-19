using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class TipoServicio
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public virtual ICollection<CategoriaServicio> CategoriaServicios { get; set; } = new List<CategoriaServicio>();

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

