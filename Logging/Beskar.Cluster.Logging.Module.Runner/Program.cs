using Beskar.Cluster.Configuration.Extensions;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Extensions;
using Beskar.Cluster.Database.Main.Contexts;
using Beskar.Cluster.Database.Telemetry.Common;
using Beskar.Cluster.Database.Telemetry.Extensions;
using Beskar.Cluster.Database.Translation.Contexts;
using Beskar.Cluster.Database.Update;
using Beskar.Cluster.Distributed.Client.Caches;
using Beskar.Cluster.Distributed.Client.Extensions;
using Beskar.Cluster.Logging.Client.Extensions;
using Beskar.Cluster.Logging.Module.Extensions;
using Beskar.Cluster.Sockets.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.UseBeskarClusterLogging();

var options = builder.Configuration
   .SetupBeskarClusterConfiguration(builder, args);

builder.Services
   .AddBeskarClusterCommonSocketServices()
   .AddBeskarClusterServerLogging()
   .AddBeskarClusterClientLogging()
   .AddBeskarClusterClientDistributed(options)
   .AddBeskarClusterTelemtryDatabaseServices()
   .AddBeskarClusterCommonDatabaseServices()
   .AddBeskarClusterDatabaseServices<DbMainContext, DbMainContextFactory>(DbContextKind.Main)
   .AddBeskarClusterDatabaseServices<DbTranslationContext, DbTranslationContextFactory>(DbContextKind.Translation);

var app = builder.Build();

// pre migration runner
await using (var scope = app.Services.CreateAsyncScope())
{
   var telemtryCreator = scope.ServiceProvider.GetRequiredService<TelemetryDatabaseCreator>();
   await telemtryCreator.EnsureCreated();
}

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
app.UseBeskarClusterServerLogging();

app.Run();