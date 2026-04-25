
using Beskar.Cluster.Logging.Protocol.Server;
using Microsoft.AspNetCore.Mvc;

namespace Beskar.Cluster.Logging.Module.Extensions;

public static class EndpointRouteBuilderExtensions
{
   extension(IEndpointRouteBuilder builder)
   {
      public IEndpointRouteBuilder UseBeskarClusterServerLogging()
      {
         builder.MapGet("/loggingWebSocket", static async (
            HttpContext context, [FromServices] LoggingServerPacketRegistry registry) =>
         {
            if (!context.WebSockets.IsWebSocketRequest)
            {
               return context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            
            using var rawWebSocket = await context.WebSockets.AcceptWebSocketAsync();
            var webSocket = new LoggingServerWebSocket(rawWebSocket, registry);

            await webSocket.StartProcessing();
            return 0;
         });
         
         return builder;
      }
   }
}