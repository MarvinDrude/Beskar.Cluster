using Beskar.Cluster.Database.File.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.File.Contexts;

public sealed partial class DbFileContext
{
   public DbSet<DbFileEntry> FileEntries => Set<DbFileEntry>();
   
   public DbSet<DbFileEntryFolder> FileEntryFolders => Set<DbFileEntryFolder>();
   
   public DbSet<DbFileStorageProvider> FileStorageProviders => Set<DbFileStorageProvider>();
   
   public DbSet<DbFileContent> FileContents => Set<DbFileContent>();
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
      
      modelBuilder.ApplyConfiguration(new DbFileEntryConfiguration());
      modelBuilder.ApplyConfiguration(new DbFileEntryFolderConfiguration());
      modelBuilder.ApplyConfiguration(new DbFileStorageProviderConfiguration());
      modelBuilder.ApplyConfiguration(new DbFileContentConfiguration());
   }
}