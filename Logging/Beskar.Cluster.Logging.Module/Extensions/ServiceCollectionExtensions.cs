using Beskar.Cluster.Logging.Protocol.Server;
using Beskar.CodeGeneration.PacketGenerator.Marker.Common;

namespace Beskar.Cluster.Logging.Module.Extensions;

public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddBeskarClusterServerLogging(this IServiceCollection services)
   {
      return services.AddSingleton(new LoggingServerPacketRegistry(registryOptions: new PacketRegistryOptions()
      {
         RunHandlersInParallel = true
      }));
   }
}