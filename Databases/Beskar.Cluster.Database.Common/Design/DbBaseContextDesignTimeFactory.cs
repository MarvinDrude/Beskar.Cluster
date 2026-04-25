using Beskar.Cluster.Configuration.Models;
using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Extensions;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Common.Utils;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Database.Common.Design;

public abstract class DbBaseContextDesignTimeFactory<TContext, TContextFactory>(DbContextKind kind)
   : IDesignTimeDbContextFactory<TContext>
   where TContext : DbBaseContext, IDbPooledContext
   where TContextFactory : DbBaseContextFactory<TContext>
{
   public TContext CreateDbContext(string[] args)
   {
      var serviceCollection = new ServiceCollection()
         .AddBeskarClusterCommonDatabaseServices()
         .AddBeskarClusterDatabaseServices<TContext, TContextFactory>(kind);

      var switchMappings = new Dictionary<string, string>
      {
         { "--ConnectionString", "Main:MainDatabaseConnectionString" } 
      };
      
      var config = new ConfigurationBuilder()
         .AddCommandLine(args, switchMappings)
         .Build();
      serviceCollection.AddSingleton<IConfiguration>(config);
      serviceCollection.Configure<MainOptions>(config.GetSection("Main"));
      
      serviceCollection.AddLogging(builder =>
      {
         builder.AddConsole();
         builder.SetMinimumLevel(LogLevel.Information);
      });
      
      var provider = serviceCollection.BuildServiceProvider();
      return AsyncSyncHelper.RunSync(async () => await provider.GetRequiredService<IDbPooledContextFactory<TContext>>().CreateAsync());
   }
}