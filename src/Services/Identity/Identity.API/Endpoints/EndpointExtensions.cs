namespace Identity.API.Endpoints;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthenticationEndpoints();
        //app.MapUserEndpoints();
        //app.MapRoleEndpoints();
        //app.MapPermissionEndpoints();

        return app;
    }
}