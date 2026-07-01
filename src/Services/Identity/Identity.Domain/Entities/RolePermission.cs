using Identity.Domain.Common;

namespace Identity.Domain.Entities;

public class RolePermission : AuditableEntity
{
    private RolePermission()
    {
    }

    public Guid RoleId { get; private set; }

    public Guid PermissionId { get; private set; }

    public Role Role { get; private set; } = default!;

    public Permission Permission { get; private set; } = default!;
}