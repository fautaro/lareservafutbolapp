using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LaReservaBackend.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registrar MediatR automáticamente desde el ensamblado de Application
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
