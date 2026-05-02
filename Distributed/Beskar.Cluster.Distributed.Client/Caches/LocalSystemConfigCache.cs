using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Beskar.Cluster.Configuration.Config;
using Beskar.Cluster.Configuration.Constants;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Main.Contexts;
using Beskar.Cluster.Database.Main.Entities.System;
using Beskar.Cluster.Database.Main.Enums.System;
using Beskar.Cluster.Jwt;
using Me.Memory.Extensions;
using Me.Memory.Results;
using Me.Memory.Results.Errors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Distributed.Client.Caches;

public sealed partial class LocalSystemConfigCache(
   IServiceProvider serviceProvider,
   ILogger<LocalSystemConfigCache> logger)
{
   private readonly IServiceProvider _serviceProvider = serviceProvider;
   private readonly ILogger<LocalSystemConfigCache> _logger = logger;
   
   private ConcurrentDictionary<string, ISystemConfig> _cache = [];
   
   public async Task Refresh(CancellationToken ct = default)
   {
      using var scope = _serviceProvider.CreateScope();
      
      var contextFactory = scope.ServiceProvider.GetRequiredService<IDbPooledContextFactory<DbMainContext>>();
      var jwtKeyResolver = scope.ServiceProvider.GetRequiredService<JwtKeyResolver>();
      
      await using var context = await contextFactory.CreateAsync(ct);
      ConcurrentDictionary<string, ISystemConfig> replacement = [];

      await foreach (var rawConfig in context.SystemConfigEntries)
      {
         var config = CreateConfigEntry(rawConfig);
         if (config.Failed)
         {
            LogFailedToCreateConfigEntry(rawConfig.Key, config.ErrorMessage);
            continue;
         }

         replacement[rawConfig.Key] = config.Success;
      }
      
      // replace all at once instead of one by one
      _cache = replacement;
      UpdateJwtOptions(jwtKeyResolver);
   }
   
   public bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value)
      where T : class, ISystemConfig
   {
      if (_cache.TryGetValue(key, out var config) && config is T casted)
      {
         value = casted;
         return true;
      }

      value = null;
      return false;
   }

   private void UpdateJwtOptions(JwtKeyResolver resolver)
   {
      if (TryGetValue<JwtSystemConfig>(ConfigurationKeys.JwtOptions, out var jwtOptions))
      {
         resolver.UpdateOptions(jwtOptions.Value);
      }
      else
      {
         LogJwtOptionsSetError();
      }
   }

   private static Result<ISystemConfig, StringError> CreateConfigEntry(DbSystemConfigEntry entry)
   {
      ISystemConfig? created = entry.Type switch
      {
         SystemConfigType.Boolean => new BooleanSystemConfig
         {
            Value = entry.Value.Deserialize<SystemConfigValueWrapper<bool>>()?.Value ?? false
         },
         SystemConfigType.String => new StringSystemConfig
         {
            Value = entry.Value.Deserialize<SystemConfigValueWrapper<string>>()?.Value ?? string.Empty
         },
         SystemConfigType.DateTimeOffset => new DateTimeOffsetSystemConfig
         {
            Value = entry.Value.Deserialize<SystemConfigValueWrapper<DateTimeOffset>>()?.Value ?? DateTimeOffset.MinValue
         },
         SystemConfigType.Float => new FloatSystemConfig
         {
            Value = entry.Value.Deserialize<SystemConfigValueWrapper<float>>()?.Value ?? 0f
         },
         SystemConfigType.Integer => new IntegerSystemConfig
         {
            Value = entry.Value.Deserialize<SystemConfigValueWrapper<int>>()?.Value ?? 0
         },
         SystemConfigType.JwtOptions => new JwtSystemConfig()
         {
            Value = entry.Value.Deserialize<SystemConfigValueWrapper<JwtOptions>>()?.Value 
               ?? throw new InvalidOperationException("Jwt options not found")
         },
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
