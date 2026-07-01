namespace Identity.API.Endpoints;

public static class PermissionEndpoints
{
    public static IEndpointRouteBuilder MapPermissionEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/permissions")
                       .WithTags("Permissions")
                       .RequireAuthorization();

        group.MapGet("/", GetPermissions);

        return app;
    }

    private static Task<IResult> GetPermissions()
        => throw new NotImplementedException();
}