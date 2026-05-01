using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLanguageConfiguration : IEntityTypeConfiguration<DbLanguage>
{
   public static readonly ValueConverter<DbLanguageId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbLanguageId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbLanguage> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();
      
      builder.Property(x => x.DisplayName)
         .HasMaxLength(256);

      builder.Property(x => x.TwoLetterCode)
         .HasMaxLength(2);

      builder.Property(x => x.Name)
         .HasMaxLength(32);
   }
}