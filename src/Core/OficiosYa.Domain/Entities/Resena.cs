using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class Resena
    {
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Calificacion { get; set; }

        [StringLength(1000)]
        public string? Comentario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public bool Activa { get; set; } = true;

        // Navegación
        public virtual Profesional Profesional { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;

        // Métodos de dominio
        public void Desactivar()
        {
            Activa = false;
        }

        public void Activar()
        {
            Activa = true;
        }

        public bool EsValida()
        {
            return Calificacion >= 1 && Calificacion <= 5 && 
                   !string.IsNullOrWhiteSpace(Comentario) && 
                   Comentario.Length <= 1000;
        }
    }
}
