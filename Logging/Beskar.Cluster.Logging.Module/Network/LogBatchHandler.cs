using System.Diagnostics;
using System.Threading.Channels;
using Beskar.Cluster.Database.Telemetry.Interfaces;
using Beskar.Cluster.Logging.Protocol.Server;
using Beskar.Cluster.Logging.Protocol.Server.Logging;
using Beskar.Cluster.Sockets.Interfaces;

namespace Beskar.Cluster.Logging.Module.Network;

public sealed class LogBatchHandler(
   LoggingServerPacketRegistry registry, 
   IServiceProvider serviceProvider) 
   : IMessageHandler
{
   private const int MaxBatchSize = 10_000;
   private static readonly TimeSpan FlushInterval = TimeSpan.FromSeconds(10);
   
   private readonly LoggingServerPacketRegistry _registry = registry;
   private readonly Channel<StructuredLogRecord> _channel = Channel.CreateUnbounded<StructuredLogRecord>();
   private readonly IServiceProvider _serviceProvider = serviceProvider;
   
   public ValueTask AttachEventHandler()
   {
      _registry.RegisterHandler<LogChunkPacket>((ref state, ref packet, ct) => Execute(state, packet, ct));
      _ = RunBatchExecution();
      
      return ValueTask.CompletedTask;
   }

   private ValueTask Execute(
      LoggingServerPacketState state, LogChunkPacket packet, CancellationToken ct)
   {
      foreach (var entry in packet.Records)
      {
         _channel.Writer.TryWrite(entry);
      }
      
      return ValueTask.CompletedTask;
   }

   private async Task RunBatchExecution(CancellationToken ct = default)
   {
      var batch = new List<StructuredLogRecord>(MaxBatchSize);
      var lastFlush = Stopwatch.GetTimestamp();

      try
      {
         while (true)
         {
            if (await _channel.Reader.WaitToReadAsync(ct))
            {
               while (_channel.Reader.TryRead(out var entry))
               {
                  batch.Add(entry);
                  if (batch.Count >= MaxBatchSize)
                  {
                     await FlushToDatabase(batch);
                     batch.Clear();
                     
                     lastFlush = Stopwatch.GetTimestamp();
                  }
               }
            }
            
            if (batch.Count > 0 && Stopwatch.GetElapsedTime(lastFlush) >= FlushInterval)
            {
               await FlushToDatabase(batch);
               batch.Clear();
               
               lastFlush = Stopwatch.GetTimestamp();
            }
         }
      }
      catch (OperationCanceledException)
      {
         // expected
      }
   }

   private async Task FlushToDatabase(List<StructuredLogRecord> batch)
   {
      using var scope = _serviceProvider.CreateScope();
      var service = scope.ServiceProvider.GetRequiredService<ILogService>();
      
      await service.InsertEntries(batch);
   }
}