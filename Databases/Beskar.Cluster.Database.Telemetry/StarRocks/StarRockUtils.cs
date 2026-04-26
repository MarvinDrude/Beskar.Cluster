namespace Beskar.Cluster.Database.Telemetry.StarRocks;

public static class StarRockUtils
{
   private const char ColumnSeparator = (char)0x01;
   private static readonly string ColumnSeperatorString = $"{ColumnSeparator}";
   
   public static string Escape(string? str)
   {
      return $"\"{str?.Replace("\"", "\"\"") ?? ""}\"";
   }

   public static string CreateRow(params string?[] values)
   {
      return string.Join(ColumnSeperatorString, values.Select(x => x ?? "\\N"));
   }

   public static IEnumerable<(string Key, string Value)> GetHeaders(string columns)
   {
      yield return ("columns", columns);
      yield return ("column_separator", ColumnSeperatorString);
      yield return ("format", "csv");
      yield return ("null_format", "\\N");
   }
}