using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosYa.Domain.Entities
{
    public class Suscripcion
    {
        public int Id { get; set; }

        [Required]
        public int ProfesionalId { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoSuscripcion { get; set; } = string.Empty; // "Basica", "Premium", "Pro"

        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaVencimiento { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } = "Activa"; // "Activa", "Vencida", "Cancelada"

        [StringLength(50)]
        public string? MetodoPago { get; set; } // "transferencia", "tarjeta", "rapipago", "pagosfaciles"

        [StringLength(100)]
        public string? CodigoPago { get; set; } // Para Rapipago/Pagos Fáciles

        public bool PagoConfirmado { get; set; } = false;

        public DateTime? FechaPago { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Navegación
        public virtual Profesional Profesional { get; set; } = null!;

        // Métodos de dominio
        public void Activar()
        {
            Estado = "Activa";
        }

        public void Cancelar()
        {
            Estado = "Cancelada";
        }

        public void MarcarComoVencida()
        {
            Estado = "Vencida";
        }

        public bool EstaActiva()
        {
            return Estado == "Activa" && FechaVencimiento > DateTime.UtcNow;
        }

        public bool EstaVencida()
        {
            return FechaVencimiento <= DateTime.UtcNow;
        }

        public int DiasRestantes()
        {
            if (EstaVencida())
                return 0;

            return (int)(FechaVencimiento - DateTime.UtcNow).TotalDays;
        }

        public void Renovar(DateTime nuevaFechaVencimiento)
        {
            FechaVencimiento = nuevaFechaVencimiento;
            Estado = "Activa";
        }
    }
}
