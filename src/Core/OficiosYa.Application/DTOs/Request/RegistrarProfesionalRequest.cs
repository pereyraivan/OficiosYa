using System.ComponentModel.DataAnnotations;

namespace OficiosYa.Application.DTOs.Request
{
    public class RegistrarProfesionalRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string Password { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? Telefono { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [StringLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]
        public string Ciudad { get; set; } = string.Empty;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        [StringLength(100, ErrorMessage = "La provincia no puede exceder 100 caracteres")]
        public string Provincia { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "El código postal no puede exceder 10 caracteres")]
        public string? CodigoPostal { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos un oficio o categoría")]
        public List<string> Oficios { get; set; } = new List<string>();

        public List<ZonaCoberturaDto> ZonasCobertura { get; set; } = new List<ZonaCoberturaDto>();

        [StringLength(50, ErrorMessage = "El método de pago no puede exceder 50 caracteres")]
        public string? MetodoPago { get; set; } // "transferencia", "tarjeta", "rapipago", "pagosfaciles"

        [StringLength(100, ErrorMessage = "El código de pago no puede exceder 100 caracteres")]
        public string? CodigoPago { get; set; } // Para Rapipago/Pagos Fáciles

        public int? MesesSuscripcion { get; set; } = 1; // Cantidad de meses de suscripción
    }

    public class ZonaCoberturaDto
    {
        [Required(ErrorMessage = "La localidad es obligatoria")]
        [StringLength(100, ErrorMessage = "La localidad no puede exceder 100 caracteres")]
        public string Localidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        [StringLength(100, ErrorMessage = "La provincia no puede exceder 100 caracteres")]
        public string Provincia { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El departamento no puede exceder 100 caracteres")]
        public string? Departamento { get; set; }

        [Required(ErrorMessage = "La latitud es obligatoria")]
        public decimal Latitud { get; set; }

        [Required(ErrorMessage = "La longitud es obligatoria")]
        public decimal Longitud { get; set; }

        [StringLength(20, ErrorMessage = "El código postal no puede exceder 20 caracteres")]
        public string? CodigoPostal { get; set; }
    }
}

