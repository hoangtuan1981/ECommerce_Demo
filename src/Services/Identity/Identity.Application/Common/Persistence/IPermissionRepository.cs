using Identity.Domain.Entities;

namespace Identity.Application.Common.Persistence;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task AddAsync(Permission permission, CancellationToken cancellationToken = default);
}