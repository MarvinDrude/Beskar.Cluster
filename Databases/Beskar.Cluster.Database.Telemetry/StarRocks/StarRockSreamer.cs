using System.Net.Http.Headers;
using System.Text;
using Beskar.Cluster.Configuration.Models;
using Me.Memory.Buffers;
using Microsoft.Extensions.Options;

namespace Beskar.Cluster.Database.Telemetry.StarRocks;

public sealed class StarRockSreamer(
   IHttpClientFactory factory,
   IOptionsMonitor<MainOptions> optionsMonitor)
{
   private readonly IOptionsMonitor<MainOptions> _optionsMonitor = optionsMonitor;
   private readonly IHttpClientFactory _factory = factory;
   
   public MainOptions Options => _optionsMonitor.CurrentValue;

   public async Task<bool> StreamData(string tableName, string columnNames, string csv, CancellationToken ct = default)
   {
      var authBytes = $"{Options.Telemetry.UserName}:{Options.Telemetry.Password}";
      var maxByteCount = Encoding.UTF8.GetMaxByteCount(authBytes.Length);
      
      var bufferOwner = maxByteCount < 512 
         ? new SpanOwner<byte>(stackalloc byte[maxByteCount])
         : new SpanOwner<byte>(maxByteCount);
      var span = bufferOwner.Span;
      
      var writtenCount = Encoding.UTF8.GetBytes(authBytes, span);
      var base64Auth = Convert.ToBase64String(span[..writtenCount]);
      
      using var client = _factory.CreateClient("StarRocks");
      using var request = new HttpRequestMessage(HttpMethod.Put, CreateUrl(tableName));
      request.Headers.ExpectContinue = true;
      
      foreach (var (key, value) in StarRockUtils.GetHeaders(columnNames))
      {
         request.Headers.Add(key, value);
      }
      
      request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64Auth}");
      request.Content = new StringContent(csv, Encoding.UTF8, "text/csv");
      
      var response = await client.SendAsync(request, ct);
      var content = await response.Content.ReadAsStringAsync(ct);
      
      return response.IsSuccessStatusCode
         && !content.Contains("FAILED") && !content.Contains("Fail");
   }

   private string CreateUrl(string tableName)
   {
      var telemetry = Options.Telemetry;
      return $"http://{telemetry.HostName}:{telemetry.HttpPort}/api/{telemetry.DatabaseName}/{tableName}/_stream_load";
   }
}