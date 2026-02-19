using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class ProfesionalOficio
    {
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
        public virtual Profesional Profesional { get; set; } = null!;
        public virtual Oficio Oficio { get; set; } = null!;

        // Métodos de dominio
        public void Desactivar()
        {
            Activo = false;
        }

        public void Activar()
        {
            Activo = true;
        }

        public void ActualizarPrecio(decimal nuevoPrecio)
        {
            if (nuevoPrecio >= 0)
                PrecioHora = nuevoPrecio;
        }

        public void ActualizarExperiencia(int anosExperiencia)
        {
            if (anosExperiencia >= 0)
                AnosExperiencia = anosExperiencia;
        }

        public bool EsExperto()
        {
            return AnosExperiencia >= 5;
        }
    }
}
