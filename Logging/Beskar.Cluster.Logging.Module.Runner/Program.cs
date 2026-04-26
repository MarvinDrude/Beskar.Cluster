using Beskar.Cluster.Configuration.Extensions;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Extensions;
using Beskar.Cluster.Database.Main.Contexts;
using Beskar.Cluster.Database.Update;
using Beskar.Cluster.Logging.Client.Extensions;
using Beskar.Cluster.Logging.Module.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.UseBeskarClusterLogging();

builder.Services
   .AddBeskarClusterServerLogging()
   .AddBeskarClusterClientLogging()
   .AddBeskarClusterCommonDatabaseServices()
   .AddBeskarClusterDatabaseServices<DbMainContext, DbMainContextFactory>(DbContextKind.Main);

builder.Configuration
   .SetupBeskarClusterConfiguration(builder, args);

var app = builder.Build();

var migrator = new MigrationRunner(app.Services);
await migrator.TryMigrate();

app.UseHttpsRedirection();

app.UseWebSockets();
app.UseBeskarClusterServerLogging();

app.Run();
