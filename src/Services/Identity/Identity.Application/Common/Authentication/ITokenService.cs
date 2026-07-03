using Identity.Domain.Entities;

namespace Identity.Application.Common.Authentication;

public interface ITokenService
{
    Task<LoginTokenResult> GenerateLoginTokenAsync(
        User user,
        CancellationToken cancellationToken = default);
}