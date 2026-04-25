using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.System;

public sealed class DbSystemConfigEntryConfiguration 
   : IEntityTypeConfiguration<DbSystemConfigEntry>
{
   public static readonly ValueConverter<Guid, DbSystemConfigEntryId> KeyConverter = new (
      id => new DbSystemConfigEntryId(id),
      id => id.Value
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