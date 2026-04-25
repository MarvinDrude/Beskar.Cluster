using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;

namespace Beskar.Cluster.Database.Common.Contexts;

public sealed class DbConnectionStringProvider : IDbConnectionStringProvider
{
   
   
   public ValueTask<string> GetConnectionString(DbContextKind kind, CancellationToken ct = default)
   {
      throw new NotImplementedException();
   }
}