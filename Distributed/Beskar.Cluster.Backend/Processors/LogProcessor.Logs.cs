using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Backend.Processors;

public sealed partial class LogProcessor
{
   [LoggerMessage(LogLevel.Information, "Start processing pipeline '{ProcessName}'...")]
   private partial void LogStart(string processName);
   
   [LoggerMessage(LogLevel.Information, "Finished processing pipeline '{ProcessName}' in {Duration}")]
   private partial void LogStop(string processName, TimeSpan duration);
}