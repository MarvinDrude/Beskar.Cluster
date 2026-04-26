using Beskar.Cluster.Logging.Module.Network;
using Beskar.Cluster.Logging.Protocol.Server;
using Beskar.Cluster.Sockets.Interfaces;
using Beskar.CodeGeneration.PacketGenerator.Marker.Common;

namespace Beskar.Cluster.Logging.Module.Extensions;

public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddBeskarClusterServerLogging(this IServiceCollection services)
   {
      services.AddSingleton<IMessageHandler, LogBatchHandler>();
      
      return services.AddSingleton(new LoggingServerPacketRegistry(registryOptions: new PacketRegistryOptions()
      {
         RunHandlersInParallel = true
      }));
   }
}