using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLangKeyConfiguration : IEntityTypeConfiguration<DbLangKey>
{
   public static readonly ValueConverter<DbLangKeyId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbLangKeyId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbLangKey> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.Property(x => x.Key)
         .HasMaxLength(256);

      builder.HasIndex(x => x.Key)
         .IsUnique(false);

      builder.HasOne(x => x.LangGroup)
         .WithMany(x => x.Keys)
         .HasForeignKey(x => x.LangGroupId);
   }
}