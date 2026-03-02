using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Application.DTOs.Request
{
    public class CrearResenaRequest
    {
        /// <summary>Nombre con el que se muestra la reseña (obligatorio si no está logueado; opcional si está logueado, se usa el nombre del usuario).</summary>
        [StringLength(100)]
        public string? AutorNombre { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5")]
        public int Calificacion { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "El comentario es obligatorio (máximo 1000 caracteres)")]
        public string Comentario { get; set; } = string.Empty;
    }
}
