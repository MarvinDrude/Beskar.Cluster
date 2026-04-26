using Beskar.Cluster.Logging.Protocol.Client;
using Beskar.CodeGeneration.PacketGenerator.Marker.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Logging.Client.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection services)
   {
      public IServiceCollection AddBeskarClusterClientLogging()
      {
         services.AddSingleton(new LoggingClientPacketRegistry(registryOptions: new PacketRegistryOptions()
         {
            RunHandlersInParallel = true
         }));
         
         return services;
      }
   }
}