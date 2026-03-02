using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    /// <summary>
    /// Trabajo o empleo publicado por un cliente para que oficiantes/profesionales se postulen.
    /// </summary>
    public class TrabajoPublicado
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Descripcion { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [Required]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string Estado { get; set; } = "Abierto"; // Abierto, Cerrado

        public bool Activo { get; set; } = true;

        public virtual ICollection<TrabajoCategoria> TrabajoCategorias { get; set; } = new List<TrabajoCategoria>();
    }
}
