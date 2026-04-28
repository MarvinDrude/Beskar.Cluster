using Beskar.Cluster.Database.Translation.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Translation.Contexts;

public sealed partial class DbTranslationContext
{
   public DbSet<DbLanguage> Languages => Set<DbLanguage>();
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.ApplyConfiguration(new DbLanguageConfiguration());
   }
}