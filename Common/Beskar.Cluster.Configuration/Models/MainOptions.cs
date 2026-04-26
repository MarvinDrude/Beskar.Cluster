namespace Beskar.Cluster.Configuration.Models;

public sealed class MainOptions
{
   public required string MainDatabaseConnectionString { get; set; }
   
   public required string LoggingServerUrl { get; set; }
}