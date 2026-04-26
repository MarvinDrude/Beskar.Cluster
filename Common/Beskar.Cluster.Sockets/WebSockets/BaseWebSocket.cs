using System.IO.Pipelines;
using System.Net.WebSockets;
using Beskar.CodeGeneration.PacketGenerator.Marker.Common;
using Beskar.CodeGeneration.PacketGenerator.Marker.Enums;
using Beskar.CodeGeneration.PacketGenerator.Marker.Interfaces;
using Me.Memory.Buffers;
using Me.Memory.Results;
using Me.Memory.Results.Errors;

namespace Beskar.Cluster.Sockets.WebSockets;

public abstract class BaseWebSocket<TRegistry>(
   WebSocket webSocket,
   TRegistry registry)
   : IAsyncDisposable
   where TRegistry : BasePacketRegistry
{
   public WebSocket WebSocket { get; } = webSocket;
   private TRegistry Registry { get; } = registry;

   private Pipe Pipe { get; } = new();
   
   private CancellationTokenSource? _cts;
   
   private Task<VoidResult<StringError>>? _readingTask;
   private Task<VoidResult<StringError>>? _packetTask;
   
   public Task StartProcessing(CancellationToken ct = default)
   {
      _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
      
      _readingTask = WebSocket.CreateListenExecution(Pipe, 4096 * 2, _cts.Token);
      _packetTask = CreatePacketExecution(_cts.Token);

      return _packetTask;
   }
   
   public ValueTask SendPacket<TPacket>(TPacket packet, CancellationToken ct = default)
      where TPacket : IPacket
   {
      // initialize with a heap memory owner from the start
      var writer = new BufferWriter<byte>(1024);
      
      try
      {
         Registry.SerializeWithHeader(ref writer, packet);

         var owner = writer.GetMemoryOwner();
         var memory = owner.Memory[..writer.Position];
         
         return WebSocket.SendAsync(memory, WebSocketMessageType.Binary, true, ct);
      }
      finally
      {
         writer.Dispose();
      }
   }

   private async Task<VoidResult<StringError>> CreatePacketExecution(CancellationToken ct = default)
   {
      var reader = Pipe.Reader;

      try
      {
         while (true)
         {
            var result = await reader.ReadAsync(ct);
            var buffer = result.Buffer;
            var totalBuffer = buffer;

            while (true)
            {
               var routeResult = await Registry.RoutePacket(buffer, ct);

               if (routeResult.ConsumedBytes > 0)
               {
                  buffer = buffer.Slice(routeResult.ConsumedBytes);
               }
               
               if (!routeResult.State.IsSuccess)
               {
                  break;
               }
               else if (routeResult.ConsumedBytes == 0)
               {
                  return new StringError("Success handle but 0 bytes consumed.");
               }
            }
            
            var consumedPosition = totalBuffer.GetPosition(totalBuffer.Length - buffer.Length);
            reader.AdvanceTo(consumedPosition, totalBuffer.End);
            
            if (result.IsCompleted)
            {
               break;
            }
         }
      }
      catch (OperationCanceledException)
      {
         return true;
      }
      catch (Exception er)
      {
         return new StringError(er.ToString());
      }
      finally
      {
         await reader.CompleteAsync();
      }

      return true;
   }

   public async ValueTask DisposeAsync()
   {
      if (_cts is not null)
      {
         await _cts.CancelAsync();
      }

      if (_readingTask is not null)
      {
         await _readingTask;
      }

      if (_packetTask is not null)
      {
         await _packetTask;
      }
      
      _cts = null;
      _readingTask = null;
      _packetTask = null;
      
      GC.SuppressFinalize(this);
   }
}