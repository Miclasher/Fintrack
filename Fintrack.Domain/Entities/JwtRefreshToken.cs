using Fintrack.Domain.Entities.Abstract;

namespace Fintrack.Domain.Entities;

public sealed class JwtRefreshToken : Entity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
