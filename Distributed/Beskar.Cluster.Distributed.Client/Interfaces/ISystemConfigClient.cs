using Beskar.Cluster.Configuration.Config;

namespace Beskar.Cluster.Distributed.Client.Interfaces;

public interface ISystemConfigClient
{
   public Task SetValue<T>(string key, T value, CancellationToken ct = default)
      where T : class, ISystemConfig;

   public Task SetValues(IEnumerable<ISystemConfig> values, CancellationToken ct = default);
   
   public Task SetValues(Dictionary<string, ISystemConfig> values, CancellationToken ct = default);
   
   public T? GetValue<T>(string key)
      where T : class, ISystemConfig;
}