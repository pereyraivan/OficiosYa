using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosYa.Domain.Entities
{
    public class PlanSuscripcion
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty; // "Basica", "Premium", "Pro"

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioMensual { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioTrimestral { get; set; } // Precio por 3 meses

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioSemestral { get; set; } // Precio por 6 meses

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioAnual { get; set; } // Precio por 12 meses

        public decimal DescuentoTrimestral { get; set; } = 0; // Porcentaje de descuento
        public decimal DescuentoSemestral { get; set; } = 0;
        public decimal DescuentoAnual { get; set; } = 0;

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Métodos de dominio
        public decimal CalcularPrecio(int meses)
        {
            return meses switch
            {
                1 => PrecioMensual,
                3 => PrecioTrimestral,
                6 => PrecioSemestral,
                12 => PrecioAnual,
                _ => PrecioMensual * meses // Precio mensual por defecto
            };
        }

        public decimal CalcularDescuento(int meses)
        {
            return meses switch
            {
                3 => DescuentoTrimestral,
                6 => DescuentoSemestral,
                12 => DescuentoAnual,
                _ => 0
            };
        }

        public decimal CalcularPrecioConDescuento(int meses)
        {
            var precioBase = CalcularPrecio(meses);
            var descuento = CalcularDescuento(meses);
            return precioBase * (1 - descuento / 100);
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public void Activar()
        {
            Activo = true;
        }
    }
}

