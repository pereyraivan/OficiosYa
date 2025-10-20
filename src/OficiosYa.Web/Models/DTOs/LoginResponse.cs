namespace OficiosYa.Web.Models.DTOs
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public UsuarioResponse? Usuario { get; set; }
        public string? Message { get; set; }
    }
}
