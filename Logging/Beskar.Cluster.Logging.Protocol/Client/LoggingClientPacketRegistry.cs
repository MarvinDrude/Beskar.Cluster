using Beskar.Cluster.Sockets.Registries;
using Beskar.CodeGeneration.PacketGenerator.Marker.Attributes;

namespace Beskar.Cluster.Logging.Protocol.Client;

[PacketRegistry<LoggingClientPacketState>]
public sealed partial class LoggingClientPacketRegistry : MessagePackPacketRegistry<LoggingClientPacketState>;

public sealed class LoggingClientPacketState
{
   
}