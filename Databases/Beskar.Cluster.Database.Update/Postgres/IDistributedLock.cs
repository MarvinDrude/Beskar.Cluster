namespace Beskar.Cluster.Database.Update.Postgres;

public interface IDistributedLock : IAsyncDisposable
{
   public Task<bool> TryAcquire(CancellationToken ct = default);
   
   public Task Acquire(CancellationToken ct = default);
}