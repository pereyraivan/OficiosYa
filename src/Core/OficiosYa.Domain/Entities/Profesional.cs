using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class Profesional
    {
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
        public virtual Usuario Usuario { get; set; } = null!;

        public virtual ICollection<ProfesionalOficio> ProfesionalOficios { get; set; } = new List<ProfesionalOficio>();
        public virtual ICollection<ProfesionalCategoria> ProfesionalCategorias { get; set; } = new List<ProfesionalCategoria>();
        public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();
        public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();
        public virtual ICollection<Suscripcion> Suscripciones { get; set; } = new List<Suscripcion>();
        public virtual ICollection<ZonaCobertura> ZonasCobertura { get; set; } = new List<ZonaCobertura>();
        public virtual ICollection<ImagenTrabajo> ImagenesTrabajo { get; set; } = new List<ImagenTrabajo>();

        // Métodos de dominio
        public void ActualizarCalificacion(decimal nuevaCalificacion, int totalResenas)
        {
            CalificacionPromedio = nuevaCalificacion;
            TotalResenas = totalResenas;
        }

        public void CambiarDisponibilidad(bool disponible)
        {
            Disponible = disponible;
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public void Activar()
        {
            Activo = true;
        }

        public bool TieneSuscripcionActiva()
        {
            return Suscripciones.Any(s => s.Estado == "Activa" && s.FechaVencimiento > DateTime.UtcNow);
        }
    }
}
