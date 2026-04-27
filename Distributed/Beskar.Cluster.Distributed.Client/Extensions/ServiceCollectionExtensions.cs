using Beskar.Cluster.Distributed.Client.Caches;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Distributed.Client.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection services)
   {
      public IServiceCollection AddBeskarClusterClientDistributed()
      {
         services.AddSingleton<LocalSystemConfigCache>();
         
         return services;
      }
   }
}