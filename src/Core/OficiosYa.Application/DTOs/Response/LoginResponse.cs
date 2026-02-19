namespace OficiosYa.Application.DTOs.Response
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public UsuarioResponse? Usuario { get; set; }
    }
}
