using Serilog;
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
            configuration
               .ReadFrom.Configuration(builder.Configuration)
               .ReadFrom.Services(services)
               .WriteTo.Console(
                  theme: AnsiConsoleTheme.Code,
                  outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
               .Enrich.FromLogContext();
         });
         
         return builder;
      }
   }
}