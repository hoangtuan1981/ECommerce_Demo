
//Inject into IServiceCollection
using Microsoft.Extensions.DependencyInjection;

namespace Product.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        //// Behaviors
        //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        //// Validators
        //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}