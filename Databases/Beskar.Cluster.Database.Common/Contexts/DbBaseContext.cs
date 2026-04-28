using System.Linq.Expressions;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Common.Interfaces.Entities;
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

   protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
   {
      base.ConfigureConventions(configurationBuilder);
      DbConventionBuilder.Configure(configurationBuilder, Kind);
   }

   public override int SaveChanges()
   {
      ApplyTrackingBehavior();
      return base.SaveChanges();
   }
   
   public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
   {
      ApplyTrackingBehavior();
      return base.SaveChangesAsync(cancellationToken);
   }
   
   private void ApplyTrackingBehavior()
   {
      var utcNow = DateTimeOffset.UtcNow;

      foreach (var entry in ChangeTracker.Entries())
      {
         if (entry.Entity is IEntityTrackable trackable)
         {
            if (entry.State is EntityState.Added)
            {
               trackable.CreatedAt = utcNow;
               trackable.UpdatedAt = utcNow;
            }
            else if (entry.State is EntityState.Modified)
            {
               trackable.UpdatedAt = utcNow;
            }
         }

         if (entry.State is EntityState.Deleted 
             && entry.Entity is IEntitySoftDeletable deletable)
         {
            entry.State = EntityState.Modified;
            
            deletable.DeletedAt = utcNow;
            deletable.IsDeleted = true;
         }
      }
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      foreach (var entity in modelBuilder.Model.GetEntityTypes())
      {
         if (typeof(IEntitySoftDeletable).IsAssignableFrom(entity.ClrType))
         {
            modelBuilder.Entity(entity.ClrType)
               .HasQueryFilter(CreateSoftDeleteFilter(entity.ClrType));
         }
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
      // only called in migration creation - otherwise always dispose async
      AsyncSyncHelper.RunSync(async () => await InvalidateState());
      base.Dispose();
      
      GC.SuppressFinalize(this);
   }

   private static LambdaExpression CreateSoftDeleteFilter(Type type)
   {
      var parameter = Expression.Parameter(type, "e");
      var property = Expression.Property(parameter, nameof(IEntitySoftDeletable.IsDeleted));
      var condition = Expression.Not(property);
      
      return Expression.Lambda(condition, parameter);
   }
}