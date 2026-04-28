using Beskar.Cluster.Database.Translation.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Translation.Contexts;

public sealed partial class DbTranslationContext
{
   public DbSet<DbLanguage> Languages => Set<DbLanguage>();
   
   public DbSet<DbLangGroup> LangGroups => Set<DbLangGroup>();
   
   public DbSet<DbLangEntry> LangEntries => Set<DbLangEntry>();
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
      
      modelBuilder.ApplyConfiguration(new DbLanguageConfiguration());
      
      modelBuilder.ApplyConfiguration(new DbLangGroupConfiguration());
      
      modelBuilder.ApplyConfiguration(new DbLangEntryConfiguration());
   }
}