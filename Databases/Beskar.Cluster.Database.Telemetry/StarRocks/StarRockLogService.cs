using Beskar.Cluster.Configuration.Models;
using Beskar.Cluster.Database.Telemetry.Interfaces;
using Beskar.Cluster.Database.Telemetry.StarRocks.Models;
using Beskar.Cluster.Logging.Protocol.Server.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Beskar.Cluster.Database.Telemetry.StarRocks;

public sealed class StarRockLogService(
   IOptionsMonitor<MainOptions> options,
   StarRockSreamer sreamer) 
   : ILogService
{
   private readonly IOptionsMonitor<MainOptions> _optionsMonitor = options;
   private readonly StarRockSreamer _sreamer = sreamer;
   
   private MainOptions Options => _optionsMonitor.CurrentValue;

   public async Task InsertEntries(List<StructuredLogRecord> entries, CancellationToken ct = default)
   {
      await _sreamer.StreamData(
         StarRockLogMap.TableName, StarRockLogMap.GetColumns(), 
         StarRockLogMap.MapToCsv(entries),
         ct);
   }
   
   public async Task EnsureCreated(CancellationToken ct = default)
   {
      await using var connection = new MySqlConnection(Options.Telemetry.CreateStarRocksConnectionString());
      await connection.OpenAsync(ct);
      
      var createTableSql = 
         $"""
         -- nosqlhighlight
         CREATE TABLE IF NOT EXISTS {StarRockLogMap.TableName} (
            timestamp DATETIME NOT NULL,
            level TINYINT NOT NULL,
            message_template TEXT NOT NULL,
            properties JSON,
            trace_id VARCHAR(128),
            span_id VARCHAR(128)
         ) 
         DUPLICATE KEY(timestamp, level)
         PARTITION BY RANGE(timestamp) ()
         DISTRIBUTED BY HASH(trace_id) BUCKETS 8
         PROPERTIES (
            "dynamic_partition.enable" = "true",
            "dynamic_partition.time_unit" = "DAY",
            "dynamic_partition.start" = "-180", -- Deletes older than 180 days (~6 months)
            "dynamic_partition.end" = "3",      -- Creates partitions for the next 3 days
            "dynamic_partition.prefix" = "p",
            "dynamic_partition.history_partition_num" = "180",
            "replication_num" = "1"
         );
         """;

      await using var command = new MySqlCommand(createTableSql, connection);
      await command.ExecuteNonQueryAsync(ct);
   }
}