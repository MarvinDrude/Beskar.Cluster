using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Jwt.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection services)
   {
      public IServiceCollection AddBeskarClusterJwtServices()
      {
         services.AddSingleton<JwtGenerator>()
            .AddSingleton<JwtKeyResolver>();
         
         return services;
      }
   }
}