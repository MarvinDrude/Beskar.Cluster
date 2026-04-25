using Beskar.Cluster.Logging.Module.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBeskarClusterServerLogging();

var app = builder.Build();
app.UseHttpsRedirection();

app.UseWebSockets();
app.UseBeskarClusterServerLogging();

app.Run();
