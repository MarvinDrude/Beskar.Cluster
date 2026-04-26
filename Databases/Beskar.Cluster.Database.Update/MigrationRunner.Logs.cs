using Beskar.Cluster.Database.Common.Enums;
using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Database.Update;

public sealed partial class MigrationRunner
{
   [LoggerMessage(LogLevel.Information, "Start getting migration lock...")]
   private partial void LogStartGettingLock();
   
   [LoggerMessage(LogLevel.Information, "Migration lock acquired.")]
   private partial void LogLockAcquired();

   [LoggerMessage(LogLevel.Information, "[{Kind}] Needed migration steps found: {Count}")]
   private partial void LogMigrationCount(DbContextKind kind, int count);

   [LoggerMessage(LogLevel.Information, "Migration completed.")]
   private partial void LogMigrationStop();
}