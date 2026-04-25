using Beskar.Cluster.Database.Common.Enums;

namespace Beskar.Cluster.Database.Common.Interfaces.Contexts;

public interface IDbConnectionStringProvider
{
   public ValueTask<string> GetConnectionString(DbContextKind kind, CancellationToken ct = default);
}