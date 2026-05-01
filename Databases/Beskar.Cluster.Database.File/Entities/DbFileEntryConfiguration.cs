using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.File.Entities;

public sealed class DbFileEntryConfiguration : IEntityTypeConfiguration<DbFileEntry>
{
   public static readonly ValueConverter<DbFileEntryId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbFileEntryId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbFileEntry> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.Property(e => e.FileName)
         .HasMaxLength(512);

      builder.Property(e => e.FileExtension)
         .HasMaxLength(16);
      
      builder.Property(e => e.MimeType)
         .HasMaxLength(128);
      
      builder.Property(e => e.DisplayName)
         .HasMaxLength(512);

      builder.HasOne(x => x.StorageProvider)
         .WithMany(x => x.Entries)
         .HasForeignKey(x => x.StorageProviderId);

      builder.HasOne(x => x.Folder)
         .WithMany(x => x.Files)
         .HasForeignKey(x => x.FolderId);

      builder.HasIndex(e => new { e.FileName, e.FileExtension, e.MimeType, e.DisplayName })
         .HasMethod("gin")
         .HasOperators(
            "gin_trgm_ops",
            "gin_trgm_ops",
            "gin_trgm_ops",
            "gin_trgm_ops");
   }
}