using Identity.Application.Common.Persistence;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    public Task AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(Role role)
    {
        throw new NotImplementedException();
    }

    public void Update(Role role)
    {
        throw new NotImplementedException();
    }
}
