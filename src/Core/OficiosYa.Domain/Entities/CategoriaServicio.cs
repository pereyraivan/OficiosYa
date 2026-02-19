using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class CategoriaServicio
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        public int TipoServicioId { get; set; }

        [StringLength(100)]
        public string? Grupo { get; set; } // Ej: "Construcción", "Salud", "Hogar", etc.

        [StringLength(10)]
        public string? Icono { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public virtual TipoServicio TipoServicio { get; set; } = null!;
        public virtual ICollection<ProfesionalCategoria> ProfesionalCategorias { get; set; } = new List<ProfesionalCategoria>();

        // Métodos de dominio
        public void Desactivar()
        {
            Activo = false;
        }

        public void Activar()
        {
            Activo = true;
        }

        public bool EsOficiante()
        {
            return TipoServicio?.Nombre.Equals("Oficiante", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public bool EsProfesional()
        {
            return TipoServicio?.Nombre.Equals("Profesional", StringComparison.OrdinalIgnoreCase) ?? false;
        }
    }
}
