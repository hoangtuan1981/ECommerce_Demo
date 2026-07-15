using Identity.API.Contracts.Authentication;
using Identity.Application.Common.Results;
using Identity.Application.Features.Authentication.Login;
using Identity.Application.Features.Authentication.Token;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Endpoints;

public static class AuthenticationEndpoints
{
    private const string RefreshTokenCookieName = "refreshToken";
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
                       .WithTags("Authentication");

        group.MapPost("/login", Login);
        //app.MapPost("/login",
        //        async (LoginRequest request) =>
        //        {
        //            return Results.Ok(new
        //            {
        //                Token = "jwt-token"
        //            });
        //        });
        //group.MapPost("/register", Register);

        //app.MapPost("/login", async (LoginCommand command, IMediator mediator) => await mediator.Send(command));


        group.MapPost("/refresh-token", RefreshToken);

        group.MapPost("/logout", Logout);

        group.MapPost("/change-password", ChangePassword);

        return app;
    }


    private static async Task<IResult> Login(
        LoginRequest request,
        IMediator mediator,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new LoginCommand(
                request.Email,
                request.Password), cancellationToken);
        // Đính kèm HttpOnly Cookie vào Response
        SetRefreshTokenCookie(context, result.Value.RefreshToken, result.Value.RefreshTokenExpiresAt);

        return Results.Ok(result);
    }

    //private static async Task<IResult> LoginV1(
    //    LoginRequest request, IMediator mediator)
    //{
    //    var result = await mediator.Send(new LoginCommand(request.Email, request.Password));
    //    return result.IsSuccess
    //        ? Results.Ok(result.Value)
    //        : Results.BadRequest(result.Errors);
    //}
    //private static async Task<IResult> Register()
    //{
    //    throw new NotImplementedException();
    //}

    private static async Task<IResult> RefreshToken(
        //RefreshTokenRequest request,
        IMediator mediator,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        // Đọc RefreshToken từ Cookie gửi lên
        if (!context.Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
        {
            return Results.Json(Result.Failure(Error.Validation("Auth.MissingCookie", "Yêu cầu không hợp lệ.")), statusCode: StatusCodes.Status401Unauthorized);
        }

        var result = await mediator.Send(
            new RefreshTokenCommand(
                refreshToken), cancellationToken);
        if (result.IsFailure)
        {
            return Results.Json(result, statusCode: StatusCodes.Status401Unauthorized);
        }

        var authResult = result.Value;

        // Ghi đè Cookie Refresh Token mới (Xoay vòng thành công)
        SetRefreshTokenCookie(context, authResult.RefreshToken, authResult.RefreshTokenExpiresAt);

        //return result.IsSuccess
        //    ? Results.Ok(result.Value)
        //    : Results.Unauthorized();
        return Results.Ok(result.Value);
    }

    private static async Task<IResult> Logout(
        HttpContext context,
        CancellationToken cancellationToken)
    {
        // Xóa Cookie khi người dùng đăng xuất
        context.Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None, // Hoặc Lax tùy thuộc vào setup Gateway
            Path = "/"
        });
        return Results.Ok(Result.Success());
    }

    private static async Task<IResult> ChangePassword()
    {
        throw new NotImplementedException();
    }

    private static void SetRefreshTokenCookie(HttpContext context, string token, DateTime expiresAt)
    {
        //var cookieOptions = new CookieOptions
        //{
        //    HttpOnly = true,   // Chống XSS: JavaScript không thể đọc được cookie này
        //    Secure = true,     // Bắt buộc chạy trên HTTPS
        //    SameSite = SameSiteMode.None, // Cần thiết khi Frontend và Gateway chạy chéo Domain/Port trên Local
        //    Expires = expiresAt,
        //    Path = "/"         // Cookie khả dụng cho toàn bộ domain
        //};
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,   // Chống XSS: JavaScript không thể đọc được cookie này
            Secure = false,     // Bắt buộc chạy trên HTTPS
            SameSite = SameSiteMode.Lax, // Cần thiết khi Frontend và Gateway chạy chéo Domain/Port trên Local
            Expires = expiresAt,
            Path = "/"         // Cookie khả dụng cho toàn bộ domain
        };
        context.Response.Cookies.Append(RefreshTokenCookieName, token, cookieOptions);
    }
}