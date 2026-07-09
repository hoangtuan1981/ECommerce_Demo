using Identity.Domain.Common;
using Identity.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

public sealed class RefreshToken : AuditableEntity
{
    private RefreshToken()
    {
    }

    public Guid UserId { get; private set; }

    public string Token { get; private set; } = default!;

    public DateTime ExpirationDate { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public string? ReplacedByToken { get; private set; }
    [NotMapped]
    public bool IsExpired
        => DateTime.UtcNow >= ExpirationDate;

    [NotMapped]
    public bool IsRevoked
        => RevokedAt.HasValue;
    [NotMapped]
    public bool IsActive
        => !IsExpired && !IsRevoked;
    public User User { get; private set; } = default!;
    public static RefreshToken Create(
        Guid userId,
        string token,
        DateTime expiresAt)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpirationDate = expiresAt
        };
    }

    public void Revoke(
        string? replacedByToken = null)
    {
        RevokedAt = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }
}