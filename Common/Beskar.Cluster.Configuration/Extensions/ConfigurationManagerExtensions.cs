namespace Beskar.Cluster.Configuration.Extensions;

public static class ConfigurationManagerExtensions
{
   extension(ConfigurationManager config)
   {
      public void SetupBeskarClusterConfiguration(string[] args)
      {
         config.Sources.Clear();
         
         config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .AddJsonFile("appsettings.User.json", optional: true, reloadOnChange: true);
      }
   }
}