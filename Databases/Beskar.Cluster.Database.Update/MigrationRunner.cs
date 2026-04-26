using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Main.Contexts;
using Beskar.Cluster.Database.Update.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Database.Update;

public sealed partial class MigrationRunner(IServiceProvider serviceProvider)
{
   private readonly IServiceProvider _serviceProvider = serviceProvider;
   private readonly ILogger<MigrationRunner> _logger = serviceProvider.GetRequiredService<ILogger<MigrationRunner>>();
   
   public async Task TryMigrate(CancellationToken ct = default)
   {
      LogStartGettingLock();
      
      await using var scope = _serviceProvider.CreateAsyncScope();
      var connectionStringProvider = scope.ServiceProvider.GetRequiredService<IDbConnectionStringProvider>();
      
      await using var migrationLock = new PostgresDistributedLock(connectionStringProvider, DbContextKind.Main, "MigrationLock");
      await migrationLock.Acquire(ct);

      LogLockAcquired();
      
      var mainContextProvider = scope.ServiceProvider.GetRequiredService<DbContextProvider<DbMainContext>>();
      var mainContext = await mainContextProvider.GetContextAsync(ct);

      if (_logger.IsEnabled(LogLevel.Information))
      {
         var count = await mainContext.Database.GetPendingMigrationsAsync(ct);
         LogMigrationCount(DbContextKind.Main, count.Count());
      }

      await mainContext.Database.MigrateAsync(ct);
      LogMigrationStop();
   }
}