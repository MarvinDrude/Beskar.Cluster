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
         return services.AddScoped<TelemetryDatabaseCreator>()
            .AddScoped<ILogService, StarRockLogService>();
      }
   }
}