namespace Beskar.Cluster.Sockets.Interfaces;

public interface IMessageHandler
{
   public ValueTask AttachEventHandler();
}