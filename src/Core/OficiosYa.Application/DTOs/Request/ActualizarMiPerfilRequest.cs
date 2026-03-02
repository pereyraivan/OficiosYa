namespace OficiosYa.Application.DTOs.Request
{
    /// <summary>
    /// Request para actualizar el perfil del profesional (panel del profesional).
    /// Todos los campos son opcionales; solo se actualizan los enviados.
    /// </summary>
    public class ActualizarMiPerfilRequest
    {
        public string? Descripcion { get; set; }
        public bool? Disponible { get; set; }
        public string? FotoPerfilBase64 { get; set; }
        public List<ZonaCoberturaDto>? ZonasCobertura { get; set; }
        public List<string>? Oficios { get; set; }
    }
}
