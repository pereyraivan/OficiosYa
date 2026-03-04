using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    /// <summary>
    /// Registro de cada vez que un usuario hace clic en "Contactar" a un profesional desde la plataforma.
    /// Sirve para conteo por plan (ej. planes con X contactos incluidos, cobro por contacto extra).
    /// </summary>
    public class ContactoProfesional
    {
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        /// <summary>Usuario que hizo clic (opcional: puede ser visitante no logueado).</summary>
        public int? UsuarioId { get; set; }

        /// <summary>Canal elegido: WhatsApp, Email, etc.</summary>
        [StringLength(20)]
        public string? TipoContacto { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        // Navegación
        public virtual Profesional Profesional { get; set; } = null!;
        public virtual Usuario? Usuario { get; set; }
    }
}
