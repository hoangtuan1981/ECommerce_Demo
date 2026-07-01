using Identity.Domain.Common;

namespace Identity.Domain.Entities;

public class Role : AuditableEntity
{
    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    private readonly List<UserRole> _userRoles = new();

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles;

    private readonly List<RolePermission> _rolePermissions = new();

    public IReadOnlyCollection<RolePermission> RolePermissions
     => _rolePermissions.AsReadOnly();
}