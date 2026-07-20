
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Common.Persistence;
using Product.Infrastructure.Persistence.Repositories;

namespace Product.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
        IConfiguration configuration)
    {
        RegisterDatabase(services, configuration);

        RegisterRepositories(services);

        return services;
    }

    private static void RegisterDatabase(
    IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddDbContext<ProductDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("ProductDb"),
                sql =>
                {
                    sql.MigrationsAssembly(typeof(ProductDbContext).Assembly.FullName);

                    sql.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
        });

        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<ProductDbContext>());
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IProductRepository, ProductRepository>();
    }
}
