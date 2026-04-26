using Beskar.Cluster.Sockets.Interfaces;

namespace Beskar.Cluster.Sockets.Services;

public sealed class MessageHandlerRegister(
   IEnumerable<IMessageHandler> handlers)
{
   private readonly IEnumerable<IMessageHandler> _handlers = handlers;

   public async Task AttachHandlers()
   {
      foreach (var handler in _handlers)
      {
         await handler.AttachEventHandler();
      }
   }
}