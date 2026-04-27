using System.Text.Json;

namespace Beskar.Cluster.Configuration.Config;

public class SystemConfig<T> : ISystemConfig
{
   public string Key { get; set; } = string.Empty;
   
   public required T Value { get; set; }
   
   public JsonElement CreateJsonElement() => JsonSerializer.SerializeToElement(new SystemConfigValueWrapper<T>(Value));
}

public class SystemConfigValueWrapper<T>(T value)
{
   public T Value { get; set; } = value;
}

public interface ISystemConfig
{
   public string Key { get; set; }
   
   public JsonElement CreateJsonElement();
}