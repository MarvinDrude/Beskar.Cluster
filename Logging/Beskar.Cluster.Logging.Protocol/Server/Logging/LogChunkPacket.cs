using Beskar.CodeGeneration.PacketGenerator.Marker.Attributes;
using Beskar.CodeGeneration.PacketGenerator.Marker.Interfaces;
using MessagePack;

namespace Beskar.Cluster.Logging.Protocol.Server.Logging;

[MessagePackObject]
[Packet(typeof(LoggingServerPacketRegistry))]
public sealed class LogChunkPacket : IPacket
{
   [Key(0)]
   public required ArraySegment<StructuredLogRecord> Records { get; init; }
}

[MessagePackObject]
public readonly struct StructuredLogRecord
{
   [Key(0)]
   public long Timestamp { get; init; }
   
   [Key(1)]
   public byte Level { get; init; }
   
   [Key(2)]
   public required string MessageTemplate { get; init; }
   
   [Key(3)]
   public required Dictionary<string, string?> Properties { get; init; }
   
   [Key(4)]
   public required string? TraceId { get; init; }
   
   [Key(5)]
   public required string? SpanId { get; init; }
}