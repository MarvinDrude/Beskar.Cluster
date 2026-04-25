using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Common.Interfaces.Contexts;

public interface IDbContextConfigurator
{
   public ValueTask Configure(DbContextKind kind, DbContextOptionsBuilder optionsBuilder, CancellationToken ct = default);

   public ValueTask UpdateConfigure(DbContextKind kind, DbBaseContext context, CancellationToken ct = default);
}