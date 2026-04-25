using Beskar.Cluster.Configuration.Extensions;
using Beskar.Cluster.Logging.Module.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBeskarClusterServerLogging();
builder.Configuration.SetupBeskarClusterConfiguration(args);

var app = builder.Build();
app.UseHttpsRedirection();

app.UseWebSockets();
app.UseBeskarClusterServerLogging();

app.Run();
