using Beskar.Cluster.Database.Telemetry.Common;
using Beskar.Cluster.Database.Telemetry.Interfaces;
using Beskar.Cluster.Database.Telemetry.StarRocks;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Database.Telemetry.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection services)
   {
      public IServiceCollection AddBeskarClusterTelemtryDatabaseServices()
      {
         services.AddHttpClient("StarRocks", _ =>
         {
            // nothing yet
         }).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler()
         {
            Expect100ContinueTimeout = TimeSpan.FromSeconds(10),
            EnableMultipleHttp2Connections = true,
            EnableMultipleHttp3Connections = true,
            AllowAutoRedirect = true,
         });
         
         return services.AddScoped<TelemetryDatabaseCreator>()
            .AddScoped<ILogService, StarRockLogService>()
            .AddSingleton<StarRockSreamer>();
      }
   }
}