namespace OficiosYa.Application.DTOs.Response
{
    public class ProfesionalResponse
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? FotoPerfil { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public int TotalResenas { get; set; }
        public bool Disponible { get; set; }
        public bool Activo { get; set; }
        public List<string> Oficios { get; set; } = new List<string>();
        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
    }
}

