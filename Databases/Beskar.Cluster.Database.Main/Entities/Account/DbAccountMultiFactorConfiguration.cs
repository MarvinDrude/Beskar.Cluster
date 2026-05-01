using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountMultiFactorConfiguration : IEntityTypeConfiguration<DbAccountMultiFactor>
{
   public static readonly ValueConverter<DbAccountMultiFactorId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbAccountMultiFactorId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbAccountMultiFactor> builder)
   {
      throw new NotImplementedException();
   }
}