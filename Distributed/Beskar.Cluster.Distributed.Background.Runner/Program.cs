using Beskar.Cluster.Configuration.Extensions;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Extensions;
using Beskar.Cluster.Database.File.Contexts;
using Beskar.Cluster.Database.Main.Contexts;
using Beskar.Cluster.Database.Translation.Contexts;
using Beskar.Cluster.Database.Update;
using Beskar.Cluster.Distributed.Background.Extensions;
using Beskar.Cluster.Distributed.Client.Caches;
using Beskar.Cluster.Distributed.Client.Extensions;
using Beskar.Cluster.Logging.Client.Extensions;
using Beskar.Cluster.Sockets.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.UseBeskarClusterLogging();

var options = builder.Configuration
   .SetupBeskarClusterConfiguration(builder, args);

builder.Services
   .AddBeskarClusterDistributedBackgroundServices()
   .AddBeskarClusterCommonSocketServices()
   .AddBeskarClusterClientLogging()
   .AddBeskarClusterClientDistributed(options)
   .AddBeskarClusterCommonDatabaseServices()
   .AddBeskarClusterDatabaseServices<DbMainContext, DbMainContextFactory>(DbContextKind.Main)
   .AddBeskarClusterDatabaseServices<DbTranslationContext, DbTranslationContextFactory>(DbContextKind.Translation)
   .AddBeskarClusterDatabaseServices<DbFileContext, DbFileContextFactory>(DbContextKind.File);

var app = builder.Build();

await app.Services.InitializeSocketHandlers();

var migrator = new MigrationRunner(app.Services);
await migrator.TryMigrate();

// post migration runner
await using (var scope = app.Services.CreateAsyncScope())
{
   var localConfigCache = scope.ServiceProvider.GetRequiredService<LocalSystemConfigCache>();
   await localConfigCache.Refresh();
}

app.UseHttpsRedirection();
app.UseWebSockets();

app.Run();