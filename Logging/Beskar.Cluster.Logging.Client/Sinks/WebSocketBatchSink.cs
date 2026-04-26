using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using Beskar.Cluster.Logging.Protocol.Client;
using Beskar.Cluster.Logging.Protocol.Server.Logging;
using Me.Memory.Buffers;
using Serilog.Core;
using Serilog.Events;

namespace Beskar.Cluster.Logging.Client.Sinks;

public sealed class WebSocketBatchSink
   : IBatchedLogEventSink
{
   private readonly Uri _uri;
   private readonly ClientWebSocket _webSocket;
   private readonly LoggingClientWebSocket _client;

   public WebSocketBatchSink(Uri uri, LoggingClientPacketRegistry registry)
   {
      _uri = uri;
      _webSocket = new ClientWebSocket();
      _client = new LoggingClientWebSocket(_webSocket, registry)
      {
         State = new LoggingClientPacketState()
      };
   }

   public async Task EmitBatchAsync(IReadOnlyCollection<LogEvent> batch)
   {
      if (_webSocket.State is not WebSocketState.Open)
      {
         await _webSocket.ConnectAsync(_uri, CancellationToken.None);
      }
      
      using var recordOwner = new MemoryOwner<StructuredLogRecord>(batch.Count);
      var records = recordOwner.Buffer;
      var index = 0;
      
      foreach (var log in batch)
      {
         records[index++] = CreateRecord(log);
      }
      
      await _client.SendPacket(new LogChunkPacket()
      {
         Records = new ArraySegment<StructuredLogRecord>(records, 0, batch.Count)
      }, CancellationToken.None);
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static StructuredLogRecord CreateRecord(LogEvent logEvent)
   {
      return new StructuredLogRecord()
      {
         Timestamp = logEvent.Timestamp.UtcTicks,
         Level = (byte)logEvent.Level,
         MessageTemplate = logEvent.MessageTemplate.Text,
         Properties = logEvent.Properties.ToDictionary(p => p.Key, p => p.Value.ToString()),
         SpanId = logEvent.TraceId?.ToHexString(),
         TraceId = logEvent.TraceId?.ToHexString()
      };
   }
}