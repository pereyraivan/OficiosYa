using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosYa.Web.Models
{
    public class Profesional
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [StringLength(200)]
        public string? FotoPerfil { get; set; }

        public decimal CalificacionPromedio { get; set; } = 0;

        public int TotalResenas { get; set; } = 0;

        public bool Disponible { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        // Navegación
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        public virtual ICollection<ProfesionalOficio> ProfesionalOficios { get; set; } = new List<ProfesionalOficio>();
        public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();
        public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();
        public virtual ICollection<Suscripcion> Suscripciones { get; set; } = new List<Suscripcion>();
    }
}
