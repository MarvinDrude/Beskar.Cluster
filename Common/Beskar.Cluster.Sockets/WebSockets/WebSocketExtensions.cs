using System.IO.Pipelines;
using System.Net.WebSockets;
using Me.Memory.Results;
using Me.Memory.Results.Errors;

namespace Beskar.Cluster.Sockets.WebSockets;

public static class WebSocketExtensions
{
   extension(WebSocket webSocket)
   {
      public async Task<VoidResult<StringError>> CreateListenExecution(
         Pipe pipe, int bufferSize = 4096, 
         CancellationToken ct = default)
      {
         try
         {
            while (webSocket.State is WebSocketState.Open)
            {
               var memory = pipe.Writer.GetMemory(bufferSize);
               var result = await webSocket.ReceiveAsync(memory, ct);

               if (result.MessageType is WebSocketMessageType.Close)
               {
                  break;
               }

               pipe.Writer.Advance(result.Count);

               if (result.EndOfMessage)
               {
                  await pipe.Writer.FlushAsync(ct);
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
            await pipe.Writer.CompleteAsync();
         }

         return true;
      }
   }
}