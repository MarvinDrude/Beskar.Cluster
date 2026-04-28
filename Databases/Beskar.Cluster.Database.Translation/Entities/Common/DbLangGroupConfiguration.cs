using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLangGroupConfiguration : IEntityTypeConfiguration<DbLangGroup>
{
   public void Configure(EntityTypeBuilder<DbLangGroup> builder)
   {
      throw new NotImplementedException();
   }
}