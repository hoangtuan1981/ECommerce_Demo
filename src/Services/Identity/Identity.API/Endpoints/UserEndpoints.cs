namespace Identity.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
                       .WithTags("Users")
                       .RequireAuthorization();

        group.MapGet("/", GetUsers);

        group.MapGet("/{id:guid}", GetUser);

        group.MapPost("/", CreateUser);

        group.MapPut("/{id:guid}", UpdateUser);

        group.MapDelete("/{id:guid}", DeleteUser);

        return app;
    }

    private static Task<IResult> GetUsers() => throw new NotImplementedException();

    private static Task<IResult> GetUser(Guid id) => throw new NotImplementedException();

    private static Task<IResult> CreateUser() => throw new NotImplementedException();

    private static Task<IResult> UpdateUser(Guid id) => throw new NotImplementedException();

    private static Task<IResult> DeleteUser(Guid id) => throw new NotImplementedException();
}