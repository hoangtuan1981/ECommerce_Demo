using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Authentication.Login;

/// <summary>
/// Authenticate user using username or email and password.
/// </summary>
public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password)
    : IRequest<Result<LoginResponse>>;