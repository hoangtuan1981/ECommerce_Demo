namespace Identity.Application.Common.Authentication;

public sealed record JwtToken(
    string AccessToken,
    DateTime ExpiresAt);