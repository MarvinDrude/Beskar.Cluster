using Me.Memory.Results.Errors;
using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Distributed.Client.Caches;

public sealed partial class LocalSystemConfigCache
{
   [LoggerMessage(LogLevel.Error, "Failed to create config entry for {Key}: {Error}")]
   private partial void LogFailedToCreateConfigEntry(string key, string error);
}
