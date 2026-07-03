namespace Identity.Application.Features.Authentication.Login;

public sealed record LoginResponse(
    Guid UserId,
    string UserName,
    string Email,
    string FullName,
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);