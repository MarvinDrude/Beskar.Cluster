using Beskar.Cluster.Sockets.Registries;
using Beskar.CodeGeneration.PacketGenerator.Marker.Attributes;

namespace Beskar.Cluster.Logging.Protocol.Server;

[PacketRegistry<LoggingServerPacketState>]
public sealed partial class LoggingServerPacketRegistry : MessagePackPacketRegistry<LoggingServerPacketState>;

public sealed class LoggingServerPacketState
{
   
}