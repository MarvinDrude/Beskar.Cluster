using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Database.Common.Contexts;

public sealed class DbContextConfigurator(
   ILoggerFactory loggerFactory,
   IDbConnectionStringProvider connectionStringProvider)
   : IDbContextConfigurator
{
   private readonly ILogger<DbContextConfigurator> _logger = loggerFactory.CreateLogger<DbContextConfigurator>();
   private readonly IDbConnectionStringProvider _connectionStringProvider = connectionStringProvider;
   
   public async ValueTask Configure(
      DbContextKind kind, DbContextOptionsBuilder optionsBuilder, CancellationToken ct = default)
   {
      optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
         .UseLoggerFactory(loggerFactory);
      
      if (_logger.IsEnabled(LogLevel.Debug))
      {
         optionsBuilder.EnableSensitiveDataLogging();
      }

      var connectionString = await _connectionStringProvider.GetConnectionString(kind, ct);
      optionsBuilder.UseNpgsql(connectionString, options =>
      {
         options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            .CommandTimeout((int)TimeSpan.FromSeconds(60).TotalSeconds);
      });
   }
}