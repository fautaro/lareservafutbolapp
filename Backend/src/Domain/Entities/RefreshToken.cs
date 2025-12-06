namespace LaReservaBackend.Domain.Entities;
public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedByIp { get; set; } = "";
    public bool Revoked { get; set; } = false;
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public bool IsActive => !Revoked && DateTime.UtcNow < ExpiresAt;
}
