using Identity.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entities;

public class RefreshToken : AuditableEntity
{
    public Guid UserId { get; private set; }

    public string Token { get; private set; } = default!;

    public DateTime ExpirationDate { get; private set; }

    public bool IsRevoked { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public User User { get; private set; } = default!;

    private RefreshToken()
    {
    }

    public RefreshToken(
        Guid userId,
        string token,
        DateTime expirationDate)
    {
        Id = Guid.NewGuid();

        UserId = userId;

        Token = token;

        ExpirationDate = expirationDate;

        IsRevoked = false;
    }

    public void Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }
}