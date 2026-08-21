namespace OficiosYa.Domain.Entities
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Usuario Usuario { get; set; } = null!;

        public bool EstaVigente => UsedAt == null && ExpiresAt > DateTime.UtcNow;

        public void MarcarComoUsado()
        {
            UsedAt = DateTime.UtcNow;
        }
    }
}
