using Identity.Domain.Entities;

namespace Identity.Application.Common.Persistence;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    void Update(User user);

    void Remove(User user);
}