using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    /// <summary>
    /// Relación entre un trabajo publicado y las categorías de servicio que requiere (ej. Albañil, Electricista).
    /// </summary>
    public class TrabajoCategoria
    {
        public int Id { get; set; }

        [Required]
        public int TrabajoPublicadoId { get; set; }

        [Required]
        public int CategoriaServicioId { get; set; }

        public virtual TrabajoPublicado TrabajoPublicado { get; set; } = null!;
        public virtual CategoriaServicio CategoriaServicio { get; set; } = null!;
    }
}
