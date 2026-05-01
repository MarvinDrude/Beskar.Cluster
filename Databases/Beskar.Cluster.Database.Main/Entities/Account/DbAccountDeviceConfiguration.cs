using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountDeviceConfiguration : IEntityTypeConfiguration<DbAccountDevice>
{
   public static readonly ValueConverter<DbAccountDeviceId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbAccountDeviceId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbAccountDevice> builder)
   {
      throw new NotImplementedException();
   }
}