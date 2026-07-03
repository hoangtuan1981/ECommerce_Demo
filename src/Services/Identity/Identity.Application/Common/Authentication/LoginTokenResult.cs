using Identity.Domain.Entities;

namespace Identity.Application.Common.Authentication;

public sealed record LoginTokenResult(
    JwtToken AccessToken,
    RefreshToken RefreshToken);