using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class Certificado
    {
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [StringLength(200)]
        public string? Institucion { get; set; }

        [StringLength(100)]
        public string? NumeroCertificado { get; set; }

        public DateTime? FechaEmision { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        [StringLength(500)]
        public string? UrlImagen { get; set; }

        public bool Verificado { get; set; } = false;

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Navegación
        public virtual Profesional Profesional { get; set; } = null!;

        // Métodos de dominio
        public void Verificar()
        {
            Verificado = true;
        }

        public void Desverificar()
        {
            Verificado = false;
        }

        public bool EstaVencido()
        {
            return FechaVencimiento.HasValue && FechaVencimiento.Value < DateTime.UtcNow;
        }

        public bool EsValido()
        {
            return !EstaVencido() && Verificado;
        }
    }
}
