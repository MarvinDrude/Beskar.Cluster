namespace Beskar.Cluster.Database.Telemetry.Interfaces;

public interface ILogService
{
   public Task EnsureCreated(CancellationToken ct = default);
}