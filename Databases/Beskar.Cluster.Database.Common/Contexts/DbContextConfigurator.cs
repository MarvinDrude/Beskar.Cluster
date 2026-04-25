using System.Data;
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

   public ValueTask UpdateConfigure(
      DbContextKind kind, DbBaseContext context, CancellationToken ct = default)
   {
      var connectionStringTask = _connectionStringProvider.GetConnectionString(kind, ct);
      if (connectionStringTask.IsCompletedSuccessfully)
      {
         UpdateConfigureInternal(connectionStringTask.Result, kind, context);
         return ValueTask.CompletedTask;
      }
      
      return Awaited();

      async ValueTask Awaited()
      {
         var connectionString = await connectionStringTask;
         UpdateConfigureInternal(connectionString, kind, context);
      }
   }

   private static void UpdateConfigureInternal(
      string connectionString, DbContextKind kind, DbBaseContext context)
   {
      var connection = context.Database.GetDbConnection();

      if (connection.State is not ConnectionState.Closed)
      {
         // edge case maybe
         connection.Close();
      }
      
      var connectionStringBefore = connection.ConnectionString;
      if (connectionStringBefore != connectionString)
      {
         connection.ConnectionString = connectionString;
      }
   }
   
   public ValueTask Configure(
      DbContextKind kind, DbContextOptionsBuilder optionsBuilder, CancellationToken ct = default)
   {
      optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
      
      if (_logger.IsEnabled(LogLevel.Debug))
      {
         optionsBuilder.EnableSensitiveDataLogging()
            .UseLoggerFactory(loggerFactory);
      }

      var connectionString = _connectionStringProvider.GetConnectionString(kind, ct);
      if (connectionString.IsCompletedSuccessfully)
      {
         ConfigureConnectionString(connectionString.Result, optionsBuilder);
         return ValueTask.CompletedTask;
      }

      return Awaited();

      async ValueTask Awaited()
      {
         var connectionStringResult = await connectionString;
         ConfigureConnectionString(connectionStringResult, optionsBuilder);
      }
   }

   private static void ConfigureConnectionString(string connectionString, DbContextOptionsBuilder optionsBuilder)
   {
      optionsBuilder.UseNpgsql(connectionString, options =>
      {
         options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            .CommandTimeout((int)TimeSpan.FromSeconds(60).TotalSeconds);
      });
   }
}