using Beskar.Cluster.Sockets.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Sockets.Extensions;

public static class ServiceProviderExtensions
{
   extension(IServiceProvider services)
   {
      public async Task InitializeSocketHandlers()
      {
         var messageHandlerRegister = services.GetRequiredService<MessageHandlerRegister>();
         await messageHandlerRegister.AttachHandlers();
      }
   }
}