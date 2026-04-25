using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Common.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Database.Common.Contexts;

public abstract class DbBaseContext(
   DbContextOptions options)
   : DbContext(options), IDbPooledContext
{
   public abstract DbContextKind Kind { get; }
   
   private IDbContextConfigurator? _configurator;
   
   public virtual ValueTask Initialize(IServiceProvider provider)
   {
      _configurator = provider.GetRequiredService<IDbContextConfigurator>();
      
      var updateTask = _configurator.UpdateConfigure(Kind, this);
      return updateTask.IsCompletedSuccessfully 
         ? ValueTask.CompletedTask : Awaited();
      
      async ValueTask Awaited()
      {
         await updateTask;
      }
   }
   
   protected virtual ValueTask InvalidateState()
   {
      _configurator = null;
      
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
      // only called in migration creation - otherwise always dispose async
      AsyncSyncHelper.RunSync(async () => await InvalidateState());
      base.Dispose();
      
      GC.SuppressFinalize(this);
   }
}