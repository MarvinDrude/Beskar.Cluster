using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLangEntryConfiguration : IEntityTypeConfiguration<DbLangEntry>
{
   public void Configure(EntityTypeBuilder<DbLangEntry> builder)
   {
      throw new NotImplementedException();
   }
}