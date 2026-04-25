using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Common.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Database.Common.Contexts;

public abstract class DbBaseContext(
   DbContextOptions options)
   : DbContext(options)
{
   public abstract DbContextKind Kind { get; }
   
   private IDbContextConfigurator? _configurator;
   
   public virtual ValueTask Initialize(IServiceProvider provider)
   {
      _configurator = provider.GetRequiredService<IDbContextConfigurator>();
      
      return ValueTask.CompletedTask;
   }
   
   protected virtual ValueTask InvalidateState()
   {
      _configurator = null;
      
      return ValueTask.CompletedTask;
   }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      base.OnConfiguring(optionsBuilder);

      if (_configurator is not null)
      {
         AsyncSyncHelper.RunSync(async () => await _configurator.Configure(Kind, optionsBuilder));
      }
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