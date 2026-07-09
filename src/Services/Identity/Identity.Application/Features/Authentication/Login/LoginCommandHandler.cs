using Identity.Application.Abstractions.Persistence;
using Identity.Application.Common.Authentication;
using Identity.Application.Common.Persistence;
using Identity.Application.Common.Results;
using Identity.Application.Errors;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.Authentication.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);

    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUserNameOrEmailAsync(
            request.UserNameOrEmail,
            cancellationToken);

        if (user is null)
            return Result.Failure<LoginResponse>(AuthenticationErrors.InvalidCredentials);

        if (!user.IsActive)
            return Result.Failure<LoginResponse>(AuthenticationErrors.UserInactive);

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Failure<LoginResponse>(AuthenticationErrors.InvalidCredentials);

        // Generate JWT Token
        var jwtToken = _jwtTokenGenerator.Generate(user);

        // Generate Refresh Token (theo interface mới)
        var generatedRefreshToken = _refreshTokenGenerator.Generate();

        // Tạo RefreshToken entity
        var refreshTokenEntity = RefreshToken.Create(
            user.Id,
            generatedRefreshToken.Token,
            generatedRefreshToken.ExpiresAt);   // ← Dùng ExpiresAt

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        // Commit
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Response
        var response = new LoginResponse(
            user.Id,
            user.UserName,
            user.Email,
            user.FullName,
            jwtToken.AccessToken,
            jwtToken.ExpiresAt,           // Giả sử JwtToken dùng ExpirationDate
            generatedRefreshToken.Token,
            generatedRefreshToken.ExpiresAt);  // ← Dùng ExpiresAt

        return Result.Success(response);
    }
}