using System.Text;
using System.Text.Json;
using Beskar.Cluster.Logging.Protocol.Server.Logging;

namespace Beskar.Cluster.Database.Telemetry.StarRocks.Models;

public sealed class StarRockLogMap
{
   public static string TableName => "log_entries";

   public static string MapToCsv(List<StructuredLogRecord> records)
   {
      var sb = new StringBuilder(records.Count * 128);
      foreach (var record in records)
      {
         sb.Append(MapToRow(record));
         sb.AppendLine();
      }
      
      return sb.ToString();
   }
   
   public static string MapToRow(StructuredLogRecord record)
   {
      var dt = new DateTimeOffset(record.Timestamp, TimeSpan.Zero)
         .ToString("yyyy-MM-dd HH:mm:ss");
      var props = JsonSerializer.Serialize(record.Properties);

      return StarRockUtils.CreateRow(
         dt, record.Level.ToString(), record.MessageTemplate, 
         props, record.TraceId, record.SpanId);
   }

   public static string GetColumns()
   {
      return "timestamp, level, message_template, properties, trace_id, span_id";
   }
}