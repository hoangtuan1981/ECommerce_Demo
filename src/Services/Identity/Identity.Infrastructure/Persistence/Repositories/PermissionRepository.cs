using Identity.Application.Common.Persistence;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        public Task AddAsync(Permission permission, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
