using Identity.Application.Common.Results;

namespace Identity.Application.Errors;

public static class AuthenticationErrors
{
    public static Error InvalidCredentials => Error.Validation(
        "Authentication.InvalidCredentials", "Invalid username/email or password.");

    public static Error UserInactive => Error.Validation(
        "Authentication.UserInactive", "User account is inactive.");
}