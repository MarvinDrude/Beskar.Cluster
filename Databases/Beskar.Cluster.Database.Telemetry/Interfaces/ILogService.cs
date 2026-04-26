using Beskar.Cluster.Logging.Protocol.Server.Logging;

namespace Beskar.Cluster.Database.Telemetry.Interfaces;

public interface ILogService
{
   public Task EnsureCreated(CancellationToken ct = default);

   public Task InsertEntries(List<StructuredLogRecord> entries, CancellationToken ct = default);
}