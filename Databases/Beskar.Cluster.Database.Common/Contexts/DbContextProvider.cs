using Beskar.Cluster.Database.Common.Interfaces.Contexts;

namespace Beskar.Cluster.Database.Common.Contexts;

public sealed class DbContextProvider<TContext>(IDbPooledContextFactory<TContext> factory) : IAsyncDisposable
   where TContext : DbBaseContext
{
   private readonly IDbPooledContextFactory<TContext> _factory = factory;
   private TContext? _context;
   
   public async ValueTask<TContext> GetContextAsync(CancellationToken ct = default)
   {
      if (_context is not null)
         return _context;
      
      _context = await _factory.CreateAsync(ct);
      return _context;
   }

   public async ValueTask DisposeAsync()
   {
      if (_context is not null) 
         await _context.DisposeAsync();
      
      _context = null;
      GC.SuppressFinalize(this);
   }
}