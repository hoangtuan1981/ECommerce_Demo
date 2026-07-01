using Identity.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entities;

public class Permission : AuditableEntity
{
    private readonly List<RolePermission> _rolePermissions = new();

    private Permission()
    {
    }

    public string Code { get; private set; } = default!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public IReadOnlyCollection<RolePermission> RolePermissions
        => _rolePermissions;
}