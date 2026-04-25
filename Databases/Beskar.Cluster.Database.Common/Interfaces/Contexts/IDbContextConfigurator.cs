using Beskar.Cluster.Database.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Common.Interfaces.Contexts;

public interface IDbContextConfigurator
{
   public ValueTask Configure(DbContextKind kind, DbContextOptionsBuilder optionsBuilder, CancellationToken ct = default);
}