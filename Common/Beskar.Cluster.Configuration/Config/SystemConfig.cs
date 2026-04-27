namespace Beskar.Cluster.Configuration.Config;

public class SystemConfig<T> : ISystemConfig
{
   public string Key { get; set; } = string.Empty;
   
   public required T Value { get; set; }
}

public interface ISystemConfig
{
   public string Key { get; set; }
}