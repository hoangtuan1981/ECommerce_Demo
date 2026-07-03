using Identity.Application.Features.Authentication.Login;
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

        //group.MapPost("/login", Login);
        //app.MapPost("/login",
        //        async (LoginRequest request) =>
        //        {
        //            return Results.Ok(new
        //            {
        //                Token = "jwt-token"
        //            });
        //        });
        //group.MapPost("/register", Register);

        app.MapPost("/login", async (LoginCommand command, IMediator mediator) => await mediator.Send(command));


        group.MapPost("/refresh-token", RefreshToken);

        group.MapPost("/logout", Logout);

        group.MapPost("/change-password", ChangePassword);

        return app;
    }

    private static async Task<IResult> Login(LoginRequest request)
    {
        return Results.Ok(new
        {
            Token = "jwt-token-test"
        });
    }

    //private static async Task<IResult> Register()
    //{
    //    throw new NotImplementedException();
    //}

    private static async Task<IResult> RefreshToken()
    {
        throw new NotImplementedException();
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