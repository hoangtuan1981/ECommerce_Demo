using Identity.Application.Common.Persistence;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    public Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
