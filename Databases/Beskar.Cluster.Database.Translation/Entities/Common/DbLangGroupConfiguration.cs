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
         .HasDefaultValueSql("uuidv7()")
         .ValueGeneratedOnAdd();

      builder.Property(x => x.Name)
         .HasMaxLength(256);
   }
}