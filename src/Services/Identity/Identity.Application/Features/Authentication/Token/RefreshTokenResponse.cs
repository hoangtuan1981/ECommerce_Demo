namespace Identity.Application.Features.Authentication.Token;

public sealed record RefreshTokenResponse(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);