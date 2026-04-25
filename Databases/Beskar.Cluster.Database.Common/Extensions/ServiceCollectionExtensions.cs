using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Common.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Database.Common.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection services)
   {
      public IServiceCollection AddBeskarClusterCommonDatabaseServices()
      {
         services.AddScoped<IDbContextConfigurator, DbContextConfigurator>();
         services.AddScoped<IDbConnectionStringProvider, DbConnectionStringProvider>();
         
         return services;
      }
      
      public IServiceCollection AddBeskarClusterDatabaseServices<TContext, TContextFactory>(DbContextKind kind)
         where TContext : DbBaseContext
         where TContextFactory : DbBaseContextFactory<TContext>
      {
         services.AddPooledDbContextFactory<TContext>((sp, options) =>
         {
            using var scope = sp.CreateScope();
            
            var configurator = scope.ServiceProvider.GetRequiredService<IDbContextConfigurator>();
            AsyncSyncHelper.RunSync(async () => await configurator.Configure(kind, options));
         }, 2048);
         
         services.AddScoped<DbContextProvider<TContext>, DbContextProvider<TContext>>();
         services.AddScoped<IDbPooledContextFactory<TContext>, TContextFactory>();
         
         return services;
      }
   }
}