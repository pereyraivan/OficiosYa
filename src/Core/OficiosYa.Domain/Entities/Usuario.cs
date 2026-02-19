using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

        [StringLength(100)]
        public string? Ciudad { get; set; }

        [StringLength(100)]
        public string? Provincia { get; set; }

        [StringLength(10)]
        public string? CodigoPostal { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool EmailVerificado { get; set; } = false;

        public bool Activo { get; set; } = true;

        public bool EsAdmin { get; set; } = false;

        // Navegación
        public virtual ICollection<Profesional> Profesionales { get; set; } = new List<Profesional>();
        public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();

        // Métodos de dominio
        public void VerificarEmail()
        {
            EmailVerificado = true;
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public void Activar()
        {
            Activo = true;
        }

        public string ObtenerNombreCompleto()
        {
            return $"{Nombre} {Apellido}".Trim();
        }
    }
}
