using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Common.Contexts;

public abstract class DbBaseContextFactory<TContext>(
   IServiceProvider serviceProvider,
   IDbContextFactory<TContext> factory)
   : IDbPooledContextFactory<TContext>
   where TContext : DbBaseContext
{
   private readonly IServiceProvider _serviceProvider = serviceProvider;
   private readonly IDbContextFactory<TContext> _factory = factory;
   
   public async ValueTask<TContext> CreateAsync(CancellationToken cancellationToken = default)
   {
      var context = await _factory.CreateDbContextAsync(cancellationToken);
      await context.Initialize(_serviceProvider);

      return context;
   }
}