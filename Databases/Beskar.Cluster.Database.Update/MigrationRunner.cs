using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Main.Contexts;
using Beskar.Cluster.Database.Update.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Database.Update;

public sealed class MigrationRunner(IServiceProvider serviceProvider)
{
   private readonly IServiceProvider _serviceProvider = serviceProvider;
   
   public async Task TryMigrate(CancellationToken ct = default)
   {
      await using var scope = _serviceProvider.CreateAsyncScope();
      var connectionStringProvider = scope.ServiceProvider.GetRequiredService<IDbConnectionStringProvider>();
      
      await using var migrationLock = new PostgresDistributedLock(connectionStringProvider, DbContextKind.Main, "MigrationLock");
      await migrationLock.Acquire(ct);

      var mainContextProvider = scope.ServiceProvider.GetRequiredService<DbContextProvider<DbMainContext>>();
      var mainContext = await mainContextProvider.GetContextAsync(ct);

      await mainContext.Database.MigrateAsync(ct);
   }
}