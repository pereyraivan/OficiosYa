namespace OficiosYa.Web.Models.DTOs
{
    public class ProfesionalResponse
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? Descripcion { get; set; }
        public string? FotoPerfil { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public int TotalResenas { get; set; }
        public bool Disponible { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
        public UsuarioResponse Usuario { get; set; } = null!;
        public List<OficioResponse> Oficios { get; set; } = new List<OficioResponse>();
        public List<CertificadoResponse> Certificados { get; set; } = new List<CertificadoResponse>();
    }

    public class OficioResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class CertificadoResponse
    {
        public int Id { get; set; }
        public int ProfesionalId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Institucion { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string? ArchivoUrl { get; set; }
    }
}
