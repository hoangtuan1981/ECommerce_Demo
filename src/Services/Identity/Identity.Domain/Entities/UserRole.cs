using Identity.Domain.Common;

namespace Identity.Domain.Entities;

public class UserRole : AuditableEntity
{
    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    public User User { get; private set; } = default!;

    public Role Role { get; private set; } = default!;

    private UserRole()
    {
    }

    public UserRole(Guid userId, Guid roleId)
    {
        Id = Guid.NewGuid();

        UserId = userId;

        RoleId = roleId;
    }
}