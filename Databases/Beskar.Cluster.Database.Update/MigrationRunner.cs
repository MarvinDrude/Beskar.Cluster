using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.File.Contexts;
using Beskar.Cluster.Database.Main.Contexts;
using Beskar.Cluster.Database.Main.Entities.System;
using Beskar.Cluster.Database.Translation.Contexts;
using Beskar.Cluster.Database.Update.Postgres;
using Beskar.Cluster.Database.Update.Seed;
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
      
      var translationContext = await MigrateDatabase<DbTranslationContext>(scope, DbContextKind.Translation, ct);
      var mainContext = await MigrateDatabase<DbMainContext>(scope, DbContextKind.Main, ct);
      var fileContext = await MigrateDatabase<DbFileContext>(scope, DbContextKind.File, ct);
      
      await SeedMainDatabase(scope, mainContext, ct);
      await SeedTranslationDatabase(scope, translationContext, ct);
      await SeedFileDatabase(scope, fileContext, ct);
      
      LogMigrationStop();
   }

   private async Task SeedMainDatabase(AsyncServiceScope scope, DbMainContext context, CancellationToken ct = default)
   {
      if (!await context.SystemConfigEntries.AnyAsync(ct))
      {
         await context.SystemConfigEntries.AddRangeAsync(DbSystemConfigEntryConfiguration.DefaultEntries, ct);
         await context.SaveChangesAsync(ct);
      }
   }

   private async Task SeedTranslationDatabase(AsyncServiceScope scope, DbTranslationContext context, CancellationToken ct = default)
   {
      await TranslationSeedRunner.Seed(scope, context, ct);
   }

   private async Task SeedFileDatabase(AsyncServiceScope scope, DbFileContext context, CancellationToken ct = default)
   {
      await Task.Delay(10, ct);
   }

   private async Task<TContext> MigrateDatabase<TContext>(AsyncServiceScope scope, DbContextKind kind, CancellationToken ct = default)
      where TContext : DbBaseContext
   {
      var contextProvider = scope.ServiceProvider.GetRequiredService<DbContextProvider<TContext>>();
      var context = await contextProvider.GetContextAsync(ct);

      if (_logger.IsEnabled(LogLevel.Information))
      {
         var count = await context.Database.GetPendingMigrationsAsync(ct);
         LogMigrationCount(kind, count.Count());
      }
      
      await context.Database.MigrateAsync(ct);
      return context;
   }
}