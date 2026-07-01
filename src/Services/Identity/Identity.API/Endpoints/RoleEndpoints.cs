namespace Identity.API.Endpoints;

public static class RoleEndpoints
{
    public static IEndpointRouteBuilder MapRoleEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/roles")
                       .WithTags("Roles")
                       .RequireAuthorization();

        group.MapGet("/", GetRoles);

        group.MapGet("/{id:guid}", GetRole);

        group.MapPost("/", CreateRole);

        group.MapPut("/{id:guid}", UpdateRole);

        group.MapDelete("/{id:guid}", DeleteRole);

        return app;
    }

    private static Task<IResult> GetRoles() => throw new NotImplementedException();

    private static Task<IResult> GetRole(Guid id) => throw new NotImplementedException();

    private static Task<IResult> CreateRole() => throw new NotImplementedException();

    private static Task<IResult> UpdateRole(Guid id) => throw new NotImplementedException();

    private static Task<IResult> DeleteRole(Guid id) => throw new NotImplementedException();
}