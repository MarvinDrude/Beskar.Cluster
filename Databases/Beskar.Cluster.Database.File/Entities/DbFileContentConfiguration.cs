using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.File.Entities;

public sealed class DbFileContentConfiguration : IEntityTypeConfiguration<DbFileContent>
{
   public static readonly ValueConverter<DbFileContentId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbFileContentId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbFileContent> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();
      
      builder.Property(e => e.Bytes)
         .HasColumnType("bytea");

      builder.HasOne(e => e.FileEntry)
         .WithOne(e => e.Content)
         .HasForeignKey<DbFileContent>(e => e.FileEntryId)
         .OnDelete(DeleteBehavior.Cascade);
   }
}