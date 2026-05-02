using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Distributed.Background.Services;

public sealed partial class JwtHostedService
{
   [LoggerMessage(LogLevel.Error, "An unexpected error occurred")]
   private partial void LogUnexpectedError(Exception ex);
}