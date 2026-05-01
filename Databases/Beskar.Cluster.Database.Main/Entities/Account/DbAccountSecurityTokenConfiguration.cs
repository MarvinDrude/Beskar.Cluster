using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountSecurityTokenConfiguration : IEntityTypeConfiguration<DbAccountSecurityToken>
{
   public static readonly ValueConverter<DbAccountSecurityTokenId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbAccountSecurityTokenId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbAccountSecurityToken> builder)
   {
      throw new NotImplementedException();
   }
}