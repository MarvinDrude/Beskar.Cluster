using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Common.Interfaces.Contexts;

public interface IDbPooledContextFactory<TContext>
   where TContext : DbContext
{
   public ValueTask<TContext> CreateAsync(CancellationToken cancellationToken = default);
}