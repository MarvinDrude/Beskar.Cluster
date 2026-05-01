using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLangGroupConfiguration : IEntityTypeConfiguration<DbLangGroup>
{
   public static readonly ValueConverter<DbLangGroupId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbLangGroupId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbLangGroup> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.Property(x => x.Name)
         .HasMaxLength(256);
   }
}