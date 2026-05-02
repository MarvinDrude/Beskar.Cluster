using Beskar.Cluster.Distributed.Background.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Distributed.Background.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection services)
   {
      public IServiceCollection AddBeskarClusterDistributedBackgroundServices()
      {
         services.AddHostedService<JwtHostedService>();
         
         return services;
      }
   }
}