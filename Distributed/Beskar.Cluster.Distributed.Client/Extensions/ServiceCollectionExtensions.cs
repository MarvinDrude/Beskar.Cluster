using Beskar.Cluster.Configuration.Models;
using Beskar.Cluster.Distributed.Client.Caches;
using Beskar.Cluster.Distributed.Client.Implementation;
using Beskar.Cluster.Distributed.Client.Interfaces;
using Beskar.Cluster.Distributed.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Beskar.Cluster.Distributed.Client.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection services)
   {
      public IServiceCollection AddBeskarClusterClientDistributed(MainOptions options)
      {
         services.AddSingleton<LocalSystemConfigCache>()
            .AddScoped<ISystemConfigClient, RedisSystemConfigClient>()
            .AddHostedService<SystemConfigHostedService>();

         var multiplexer = ConnectionMultiplexer.Connect(options.CacheConfiguration);
         services.AddSingleton<IConnectionMultiplexer>(multiplexer);
         
         return services;
      }
   }
}