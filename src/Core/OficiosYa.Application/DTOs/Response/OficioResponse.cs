namespace OficiosYa.Application.DTOs.Response
{
    public class OficioResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Icono { get; set; }
        public string? Color { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

