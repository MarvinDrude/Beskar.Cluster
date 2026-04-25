
using Beskar.Cluster.Configuration.Models;

namespace Beskar.Cluster.Configuration.Extensions;

public static class ConfigurationManagerExtensions
{
   extension(ConfigurationManager config)
   {
      public MainOptions SetupBeskarClusterConfiguration(IHostApplicationBuilder builder, string[] args)
      {
         config.Sources.Clear();
         
         config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .AddJsonFile("appsettings.User.json", optional: true, reloadOnChange: true);
         
         builder.Services.Configure<MainOptions>(config.GetSection("Main"));
         
         var options = config.GetRequiredSection("Main").Get<MainOptions>();
         return options ?? throw new InvalidOperationException("Main options not found");
      }
   }
}