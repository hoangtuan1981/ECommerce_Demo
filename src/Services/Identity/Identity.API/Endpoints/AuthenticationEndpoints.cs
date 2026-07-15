using Identity.API.Contracts.Authentication;
using Identity.Application.Features.Authentication.Login;
using Identity.Application.Features.Authentication.Token;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Identity.API.Endpoints;

public static class AuthenticationEndpoints
{
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
        IMediator mediator)
    {
        var result = await mediator.Send(
            new LoginCommand(
                request.Email,
                request.Password));

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
        RefreshTokenRequest request,
        IMediator mediator)
    {
        var result = await mediator.Send(
            new RefreshTokenCommand(
                request.RefreshToken));

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.Unauthorized();
    }

    private static async Task<IResult> Logout()
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> ChangePassword()
    {
        throw new NotImplementedException();
    }
}