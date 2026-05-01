using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.File.Entities;

public sealed class DbFileStorageProviderConfiguration : IEntityTypeConfiguration<DbFileStorageProvider>
{
   public static readonly ValueConverter<DbFileStorageProviderId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbFileStorageProviderId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbFileStorageProvider> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.Property(e => e.DisplayName)
         .HasMaxLength(512);

      builder.Property(e => e.ConfigurationJson)
         .HasColumnType("jsonb");
   }
}