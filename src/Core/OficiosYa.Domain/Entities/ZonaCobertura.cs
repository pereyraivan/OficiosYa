using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class ZonaCobertura
    {
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        [StringLength(100)]
        public string Localidad { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Provincia { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Departamento { get; set; }

        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }

        [StringLength(20)]
        public string? CodigoPostal { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        // Navegación
        public virtual Profesional Profesional { get; set; } = null!;

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

