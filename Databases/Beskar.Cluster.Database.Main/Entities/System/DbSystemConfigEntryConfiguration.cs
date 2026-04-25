using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.System;

public sealed class DbSystemConfigEntryConfiguration 
   : IEntityTypeConfiguration<DbSystemConfigEntry>
{
   public static readonly ValueConverter<DbSystemConfigEntryId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbSystemConfigEntryId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbSystemConfigEntry> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .ValueGeneratedOnAdd();

      builder.HasKey(e => e.Key);

      builder.Property(e => e.Key)
         .HasMaxLength(512);

      builder.Property(e => e.Value)
         .HasColumnType("jsonb")
         .IsRequired();
   }
}