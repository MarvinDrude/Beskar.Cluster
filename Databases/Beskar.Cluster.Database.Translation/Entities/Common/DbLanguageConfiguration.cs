using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLanguageConfiguration : IEntityTypeConfiguration<DbLanguage>
{
   public void Configure(EntityTypeBuilder<DbLanguage> builder)
   {
      throw new NotImplementedException();
   }
}