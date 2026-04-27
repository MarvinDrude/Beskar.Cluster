namespace Beskar.Cluster.Configuration.Models;

public sealed class MainOptions
{
   /// <summary>
   /// Example: Server=localhost;Database=MainDatabase;User Id=sa;Password=;
   /// </summary>
   public required string MainDatabaseConnectionString { get; set; }
   
   /// <summary>
   /// Example: ws://localhost:5000
   /// </summary>
   public required string LoggingServerUrl { get; set; }
   
   /// <summary>
   /// Examples: server1:6379,server2:6379,server3:6379...
   /// </summary>
   public required string CacheConfiguration { get; set; }
   
   public required TelemetryOptions Telemetry { get; set; }
}