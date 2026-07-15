using Identity.Application.Abstractions.Persistence;
using Identity.Application.Common.Authentication;
using Identity.Application.Common.Persistence;
using Identity.Application.Common.Results;
using Identity.Application.Errors;
using MediatR;

namespace Identity.Application.Features.Authentication.Token;

public sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RefreshTokenResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository
            .GetByTokenAsync(
                request.RefreshToken,
                cancellationToken);

        if (refreshToken is null)
        {
            return Result.Failure<RefreshTokenResponse>(
                AuthenticationErrors.InvalidRefreshToken);
        }

        if (refreshToken.IsRevoked)
        {
            return Result.Failure<RefreshTokenResponse>(
                AuthenticationErrors.InvalidRefreshToken);
        }

        if (refreshToken.ExpirationDate <= DateTime.UtcNow)
        {
            return Result.Failure<RefreshTokenResponse>(
                AuthenticationErrors.RefreshTokenExpired);
        }

        var user = await _userRepository.GetByIdAsync(
            refreshToken.UserId,
            cancellationToken);

        if (user is null || !user.IsActive)
        {
            return Result.Failure<RefreshTokenResponse>(
                AuthenticationErrors.InvalidCredentials);
        }

        // Generate new JWT
        var jwtToken = _jwtTokenGenerator.Generate(user);

        // Rotate refresh token
        refreshToken.Revoke();

        var newRefreshToken = _refreshTokenGenerator.Generate();

        var refreshTokenEntity = RefreshToken.Create(
            user.Id,
            newRefreshToken.Token,
            newRefreshToken.ExpiresAt);

        await _refreshTokenRepository.AddAsync(
            refreshTokenEntity,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new RefreshTokenResponse(
            jwtToken.AccessToken,
            jwtToken.ExpiresAt,
            newRefreshToken.Token,
            newRefreshToken.ExpiresAt);

        return Result.Success(response);
    }
}