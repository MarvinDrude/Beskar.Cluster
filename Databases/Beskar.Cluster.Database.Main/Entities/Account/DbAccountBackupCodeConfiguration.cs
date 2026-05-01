using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountBackupCodeConfiguration : IEntityTypeConfiguration<DbAccountBackupCode>
{
   public static readonly ValueConverter<DbAccountBackupCodeId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbAccountBackupCodeId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbAccountBackupCode> builder)
   {
      throw new NotImplementedException();
   }
}