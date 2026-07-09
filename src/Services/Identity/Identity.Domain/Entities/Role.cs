using Identity.Domain.Common;
using Identity.Domain.Entities;

public sealed class Role : AuditableEntity
{
    private readonly List<UserRole> _userRoles = new();

    private readonly List<RolePermission> _rolePermissions = new();

    private Role()
    {
    }

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public IReadOnlyCollection<UserRole> UserRoles
        => _userRoles.AsReadOnly();

    public IReadOnlyCollection<RolePermission> RolePermissions
        => _rolePermissions.AsReadOnly();

    public static Role Create(
        string name,
        string description)
    {
        return new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }

    public void Update(
        string name,
        string description)
    {
        Name = name;
        Description = description;
    }
}