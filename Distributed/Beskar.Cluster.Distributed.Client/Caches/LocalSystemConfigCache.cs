using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Beskar.Cluster.Configuration.Config;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Main.Contexts;
using Beskar.Cluster.Database.Main.Entities.System;
using Beskar.Cluster.Database.Main.Enums.System;
using Me.Memory.Extensions;
using Me.Memory.Results;
using Me.Memory.Results.Errors;
using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Distributed.Client.Caches;

public sealed partial class LocalSystemConfigCache(
   IDbPooledContextFactory<DbMainContext> contextFactory,
   ILogger<LocalSystemConfigCache> logger)
{
   private readonly IDbPooledContextFactory<DbMainContext> _contextFactory = contextFactory;
   private readonly ILogger<LocalSystemConfigCache> _logger = logger;
   
   private ConcurrentDictionary<string, ISystemConfig> _cache = [];
   
   public async Task Refresh(CancellationToken ct = default)
   {
      await using var context = await _contextFactory.CreateAsync(ct);
      ConcurrentDictionary<string, ISystemConfig> replacement = [];

      await foreach (var rawConfig in context.SystemConfigEntries)
      {
         var config = CreateConfigEntry(rawConfig);
         if (config.Failed)
         {
            LogFailedToCreateConfigEntry(rawConfig.Key, config.ErrorMessage);
            continue;
         }

         _cache[rawConfig.Key] = config.Success;
      }
      
      // replace all at once instead of one by one
      _cache = replacement;
   }
   
   public bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value)
   {
      if (_cache.TryGetValue(key, out var config) && config is T casted)
      {
         value = casted;
         return true;
      }

      value = default;
      return false;
   }

   private static Result<ISystemConfig, StringError> CreateConfigEntry(DbSystemConfigEntry entry)
   {
      ISystemConfig? created = entry.Type switch
      {
         SystemConfigType.Boolean => new BooleanSystemConfig { Value = entry.Value.GetBoolean() },
         SystemConfigType.String => new StringSystemConfig { Value = entry.Value.GetString() },
         SystemConfigType.DateTimeOffset => new DateTimeOffsetSystemConfig { Value = entry.Value.GetDateTimeOffset() },
         SystemConfigType.Float => new FloatSystemConfig { Value = entry.Value.GetSingle() },
         SystemConfigType.Integer => new IntegerSystemConfig { Value = entry.Value.GetInt32() },
         _ => null
      };

      if (created is null)
      {
         return new StringError($"{entry.Id} is type {entry.Type} but JSON value is malformed: {entry.Value.ToString()}.");
      }

      created.Key = entry.Key;
      return new Result<ISystemConfig, StringError>(created);
   }
}
