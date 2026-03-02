namespace OficiosYa.Application.DTOs.Response
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Provincia { get; set; }
        public string? CodigoPostal { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool EmailVerificado { get; set; }
        public bool Activo { get; set; }
        public bool EsAdmin { get; set; }
        public bool EsProfesional { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
    }
}
