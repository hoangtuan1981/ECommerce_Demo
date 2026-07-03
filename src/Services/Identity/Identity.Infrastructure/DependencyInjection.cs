using Identity.Application.Common.Authentication;
using Identity.Application.Common.Persistence;
using Identity.Infrastructure.Authentication;
using Identity.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        RegisterDatabase(services, configuration);

        RegisterRepositories(services);

        RegisterAuthentication(services);

        return services;
    }

    private static void RegisterDatabase(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("IdentityDb"),
                sql =>
                {
                    sql.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName);

                    sql.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
        });

        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<IdentityDbContext>());
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }

    private static void RegisterAuthentication(IServiceCollection services)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }
}