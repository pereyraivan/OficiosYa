using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class ProfesionalCategoria
    {
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        public int CategoriaServicioId { get; set; }

        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        // Navegación
        public virtual Profesional Profesional { get; set; } = null!;
        public virtual CategoriaServicio CategoriaServicio { get; set; } = null!;

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
