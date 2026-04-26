namespace Beskar.Cluster.Configuration.Models;

public sealed class TelemetryOptions
{
   public required string HostName { get; set; }
   public required string HttpPort { get; set; }
   
   public required string DatabaseName { get; set; }
   public required string Port { get; set; }
   
   public required string UserName { get; set; }
   public required string Password { get; set; }

   public string CreateStarRocksConnectionString()
   {
      return $"Server={HostName};Port={Port};Database={DatabaseName};Uid={UserName};Pwd={Password};";
   }
}