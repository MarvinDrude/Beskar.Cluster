using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Common.Contexts;

public abstract class DbBaseContext(DbContextOptions options)
   : DbContext(options)
{
   public virtual ValueTask Initialize(IServiceProvider provider)
   {
      return ValueTask.CompletedTask;
   }
   
   protected virtual ValueTask InvalidateState()
   {
      return ValueTask.CompletedTask;
   }

   public override async ValueTask DisposeAsync()
   {
      await InvalidateState();
      await base.DisposeAsync();
      
      GC.SuppressFinalize(this);
   }

   public override void Dispose()
   {
      base.Dispose();
      throw new InvalidOperationException("DisposeAsync must be called.");
   }
}