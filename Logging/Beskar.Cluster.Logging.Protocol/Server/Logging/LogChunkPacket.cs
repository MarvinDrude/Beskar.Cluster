using Beskar.CodeGeneration.PacketGenerator.Marker.Attributes;
using Beskar.CodeGeneration.PacketGenerator.Marker.Interfaces;
using MessagePack;

namespace Beskar.Cluster.Logging.Protocol.Server.Logging;

[MessagePackObject]
[Packet(typeof(LoggingServerPacketRegistry))]
public sealed class LogChunkPacket : IPacket
{
   [Key(0)]
   public required StructuredLogRecord[] Records { get; init; }
}

[MessagePackObject]
public readonly struct StructuredLogRecord
{
   [Key(0)]
   public long Timestamp { get; init; }
   
   [Key(1)]
   public int Level { get; init; }
   
   
}