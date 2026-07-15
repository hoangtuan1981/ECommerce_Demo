using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Authentication.Token;

public sealed record RefreshTokenCommand(
    string RefreshToken)
    : IRequest<Result<RefreshTokenResponse>>;