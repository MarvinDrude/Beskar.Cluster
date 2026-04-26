using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Telemetry.Interfaces;
using Beskar.Cluster.Database.Update.Postgres;

namespace Beskar.Cluster.Database.Telemetry.Common;

public sealed class TelemetryDatabaseCreator(
   IDbConnectionStringProvider connectionStringProvider,
   ILogService logService)
{
   private readonly IDbConnectionStringProvider _connectionStringProvider = connectionStringProvider;
   private readonly ILogService _logService = logService;
   
   public async Task EnsureCreated(CancellationToken ct = default)
   {
      await using var migrationLock = new PostgresDistributedLock(_connectionStringProvider, DbContextKind.Main, "MigrationLock");
      await migrationLock.Acquire(ct);
      
      await _logService.EnsureCreated(ct);
   }
}