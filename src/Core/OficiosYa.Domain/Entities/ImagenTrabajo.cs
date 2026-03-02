using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    /// <summary>
    /// Imagen de un trabajo realizado (portfolio) del profesional/oficiante.
    /// Independiente de Certificado: certificados = formación; imágenes = obras realizadas.
    /// </summary>
    public class ImagenTrabajo
    {
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        [StringLength(500)]
        public string UrlImagen { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Titulo { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public int Orden { get; set; } = 0;

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        public virtual Profesional Profesional { get; set; } = null!;
    }
}
