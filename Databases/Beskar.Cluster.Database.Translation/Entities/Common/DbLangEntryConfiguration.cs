using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLangEntryConfiguration : IEntityTypeConfiguration<DbLangEntry>
{
   public static readonly ValueConverter<DbLangEntryId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbLangEntryId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbLangEntry> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql("gen_random_uuid()")
         .ValueGeneratedOnAdd();
      
      builder.Property(x => x.Text)
         .HasColumnType("text")
         .HasMaxLength(65535);
      
      builder.HasOne(x => x.LangKey)
         .WithMany(x => x.Entries)
         .HasForeignKey(x => x.LangKeyId);
      
      builder.HasOne(x => x.Language)
         .WithMany()
         .HasForeignKey(x => x.LanguageId);
   }
}