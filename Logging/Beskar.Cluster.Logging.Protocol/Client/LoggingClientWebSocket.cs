using System.Net.WebSockets;
using Beskar.Cluster.Sockets.WebSockets;

namespace Beskar.Cluster.Logging.Protocol.Client;

public sealed class LoggingClientWebSocket(
   ClientWebSocket webSocket, 
   LoggingClientPacketRegistry registry) 
   : BaseWebSocket<LoggingClientPacketRegistry>(webSocket, registry)
{
   
}