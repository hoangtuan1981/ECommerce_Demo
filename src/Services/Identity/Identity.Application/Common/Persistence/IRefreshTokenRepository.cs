using Identity.Domain.Entities;

namespace Identity.Application.Common.Persistence;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);

    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
}
