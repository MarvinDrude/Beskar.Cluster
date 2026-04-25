using Beskar.Cluster.Database.Main.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Main.Contexts;

public sealed partial class DbMainContext
{
   public DbSet<DbSystemConfigEntry> SystemConfigEntries => Set<DbSystemConfigEntry>();
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.ApplyConfiguration(new DbSystemConfigEntryConfiguration());
   }
}