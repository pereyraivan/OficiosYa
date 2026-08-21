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

        [StringLength(1000, ErrorMessage = "El comentario no puede superar 1000 caracteres")]
        public string? Comentario { get; set; }
    }
}
