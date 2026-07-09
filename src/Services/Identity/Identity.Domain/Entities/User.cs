using Identity.Domain.Common;

namespace Identity.Domain.Entities;

public class User : AggregateRoot
{
    public string UserName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string FullName { get; private set; } = default!;
    public bool IsActive { get; private set; }
    private readonly List<UserRole> _userRoles = new();
    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<UserRole> UserRoles
        => _userRoles.AsReadOnly();

    public IReadOnlyCollection<RefreshToken> RefreshTokens
        => _refreshTokens.AsReadOnly();

    public static User Create(
        string userName,
        string email,
        string passwordHash,
        string fullName)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            Email = email,
            PasswordHash = passwordHash,
            FullName = fullName,
            IsActive = true
        };
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void AddRole(Guid roleId)
    {
        if (_userRoles.Any(x => x.RoleId == roleId))
            return;

        _userRoles.Add(
            new UserRole(Id, roleId));
    }

    public RefreshToken AddRefreshToken(
        string token,
        DateTime expiresAt)
    {
        var refreshToken =
            RefreshToken.Create(
                Id,
                token,
                expiresAt);

        _refreshTokens.Add(refreshToken);

        return refreshToken;
    }
}