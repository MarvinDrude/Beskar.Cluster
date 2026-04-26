using System.Net.WebSockets;
using Beskar.Cluster.Sockets.WebSockets;

namespace Beskar.Cluster.Logging.Protocol.Server;

public sealed class LoggingServerWebSocket(
   WebSocket webSocket, 
   LoggingServerPacketRegistry registry) 
   : BaseWebSocket<LoggingServerPacketRegistry, LoggingServerPacketState>(webSocket, registry)
{
   
}