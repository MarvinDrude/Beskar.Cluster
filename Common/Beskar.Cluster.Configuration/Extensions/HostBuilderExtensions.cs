using Beskar.Cluster.Configuration.Models;
using Beskar.Cluster.Logging.Client.Sinks;
using Beskar.Cluster.Logging.Protocol.Client;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Configuration;
using Serilog.Sinks.SystemConsole.Themes;

namespace Beskar.Cluster.Configuration.Extensions;

public static class HostBuilderExtensions
{
   extension(IHostApplicationBuilder builder)
   {
      public IHostApplicationBuilder UseBeskarClusterLogging()
      {
         builder.Services.AddSerilog((services, configuration) =>
         {
            using var scope = services.CreateScope();
            var options = scope.ServiceProvider.GetRequiredService<IOptions<MainOptions>>().Value;
            var registry = scope.ServiceProvider.GetRequiredService<LoggingClientPacketRegistry>();
            
            configuration
               .ReadFrom.Configuration(builder.Configuration)
               .ReadFrom.Services(services)
               .WriteTo.Console(
                  theme: AnsiConsoleTheme.Code,
                  outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
               .WriteTo.Sink(new WebSocketBatchSink(new Uri(options.LoggingServerUrl), registry), new BatchingOptions()
               {
                  BatchSizeLimit = 512,
                  BufferingTimeLimit = TimeSpan.FromSeconds(10),
                  EagerlyEmitFirstEvent = true
               })
               .Enrich.FromLogContext();
         });
         
         return builder;
      }
   }
}