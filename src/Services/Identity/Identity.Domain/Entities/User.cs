using Identity.Domain.Common;

namespace Identity.Domain.Entities;

public class User : AggregateRoot
{
    public string Email { get; private set; } = default!;

    public string PasswordHash { get; private set; } = default!;

    public string FullName { get; private set; } = default!;

    public bool IsActive { get; private set; }

    private readonly List<UserRole> _userRoles = new();

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles;

    private readonly List<RefreshToken> _refreshTokens = new();

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;
}