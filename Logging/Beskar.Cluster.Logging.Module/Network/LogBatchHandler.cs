using Beskar.Cluster.Logging.Protocol.Server;
using Beskar.Cluster.Logging.Protocol.Server.Logging;
using Beskar.Cluster.Sockets.Interfaces;

namespace Beskar.Cluster.Logging.Module.Network;

public sealed class LogBatchHandler(
   LoggingServerPacketRegistry registry) 
   : IMessageHandler
{
   private readonly LoggingServerPacketRegistry _registry = registry;
   
   public ValueTask AttachEventHandler()
   {
      _registry.RegisterHandler<LogChunkPacket>((ref state, ref packet, ct) => Execute(state, packet, ct));
      return ValueTask.CompletedTask;
   }

   private static async ValueTask Execute(
      LoggingServerPacketState state, LogChunkPacket packet, CancellationToken ct)
   {
      Console.WriteLine();
   }
}