using Beskar.Cluster.Sockets.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Sockets.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection services)
   {
      public IServiceCollection AddBeskarClusterCommonSocketServices()
      {
         services.AddSingleton<MessageHandlerRegister>();
         return services;
      }
   }
}