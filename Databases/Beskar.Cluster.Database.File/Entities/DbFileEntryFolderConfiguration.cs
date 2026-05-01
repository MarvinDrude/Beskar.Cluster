using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.File.Entities;

public sealed class DbFileEntryFolderConfiguration : IEntityTypeConfiguration<DbFileEntryFolder>
{
   public static readonly ValueConverter<DbFileEntryFolderId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbFileEntryFolderId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbFileEntryFolder> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.Property(e => e.Name)
         .HasMaxLength(512);
      
      builder.HasOne(e => e.ParentFolder)
         .WithMany(x => x.SubFolders)
         .HasForeignKey(x => x.ParentFolderId);
      
      builder.HasIndex(e => e.Name)
         .HasMethod("gin")
         .HasOperators("gin_trgm_ops");
   }
}