using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountConfiguration : IEntityTypeConfiguration<DbAccount>
{
   public static readonly ValueConverter<DbAccountId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbAccountId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbAccount> builder)
   {
      throw new NotImplementedException();
   }
}