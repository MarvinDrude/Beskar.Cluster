using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountExternalConfiguration : IEntityTypeConfiguration<DbAccountExternal>
{
   public static readonly ValueConverter<DbAccountExternalId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbAccountExternalId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbAccountExternal> builder)
   {
      throw new NotImplementedException();
   }
}