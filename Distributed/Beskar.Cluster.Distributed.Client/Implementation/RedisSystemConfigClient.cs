using Beskar.Cluster.Configuration.Config;
using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Main.Contexts;
using Beskar.Cluster.Distributed.Client.Caches;
using Beskar.Cluster.Distributed.Client.Constants;
using Beskar.Cluster.Distributed.Client.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Beskar.Cluster.Distributed.Client.Implementation;

public sealed class RedisSystemConfigClient(
   IConnectionMultiplexer connectionMultiplexer,
   DbContextProvider<DbMainContext> mainContextProvider,
   LocalSystemConfigCache localSystemConfigCache,
   ILogger<RedisSystemConfigClient> logger)
   : ISystemConfigClient
{
   private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
   private readonly ISubscriber _database = connectionMultiplexer.GetSubscriber();

   private readonly DbContextProvider<DbMainContext> _mainContextProvider = mainContextProvider;
   private readonly LocalSystemConfigCache _localSystemConfigCache = localSystemConfigCache;
   private readonly ILogger<RedisSystemConfigClient> _logger = logger;
   
   public async Task SetValue<T>(string key, T value, CancellationToken ct = default)
      where T : class, ISystemConfig
   {
      await using var context = await _mainContextProvider.GetContextAsync(ct);
      var jsonElement = value.CreateJsonElement();
      
      await context.SystemConfigEntries.Where(x => x.Key == key)
         .ExecuteUpdateAsync(s => s.SetProperty(x => x.Value, jsonElement), ct);
      
      await _database.PublishAsync(DistributedChannels.RefreshSystemConfigChannel, key);
   }

   public Task SetValues(IEnumerable<ISystemConfig> values, CancellationToken ct = default)
   {
      var lookup = values.ToDictionary(x => x.Key);
      return SetValues(lookup, ct);
   }
   
   public async Task SetValues(Dictionary<string, ISystemConfig> values, CancellationToken ct = default)
   {
      var keys = values.Keys.ToArray();
      
      await using var context = await _mainContextProvider.GetContextAsync(ct);
      var entries = await context.SystemConfigEntries.AsTracking()
         .Where(x => Enumerable.Contains(keys, x.Key))
         .ToListAsync(ct);

      foreach (var entry in entries)
      {
         if (values.TryGetValue(entry.Key, out var value))
         {
            entry.Value = value.CreateJsonElement();
         }
      }
      
      await context.SaveChangesAsync(ct);
      await _database.PublishAsync(DistributedChannels.RefreshSystemConfigChannel, string.Join(',', keys));
   }

   public T? GetValue<T>(string key)
      where T : class, ISystemConfig
   {
      return _localSystemConfigCache.TryGetValue<T>(key, out var cachedValue) 
         ? cachedValue 
         : null;
   }
}