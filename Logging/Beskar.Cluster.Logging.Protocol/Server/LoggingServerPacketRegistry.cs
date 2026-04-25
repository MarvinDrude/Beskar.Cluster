using Beskar.Cluster.Sockets.Registries;
using Beskar.CodeGeneration.PacketGenerator.Marker.Attributes;

namespace Beskar.Cluster.Logging.Protocol.Server;

[PacketRegistry]
public sealed partial class LoggingServerPacketRegistry : MessagePackPacketRegistry;