using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LaReservaBackend.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Registrar todos los repositorios automáticamente
   var assembly = Assembly.GetExecutingAssembly();
        
        var repositoryTypes = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
     .ToList();

 foreach (var repositoryType in repositoryTypes)
      {
            var interfaceType = repositoryType.GetInterfaces()
        .FirstOrDefault(i => i.Name == $"I{repositoryType.Name}");
       
            if (interfaceType != null)
     {
    services.AddScoped(interfaceType, repositoryType);
  }
        }

 return services;
    }
}
