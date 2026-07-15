namespace Identity.Application.Errors;

public static class AuthenticationErrors
{
    public static Error InvalidCredentials => Error.Validation(
        "Authentication.InvalidCredentials", "Invalid username/email or password.");

    public static Error UserInactive => Error.Validation(
        "Authentication.UserInactive", "User account is inactive.");

    public static readonly Error InvalidRefreshToken =
    new(
        "Authentication.InvalidRefreshToken",
        "Refresh token is invalid.");

    public static readonly Error RefreshTokenExpired =
        new(
            "Authentication.RefreshTokenExpired",
            "Refresh token has expired.");
}