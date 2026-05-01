using Beskar.Cluster.Database.Common.Constants;
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
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.Property(x => x.Fingerprint)
         .HasMaxLength(2048);
      
      builder.Property(x => x.DeviceName)
         .HasMaxLength(256);
      
      builder.Property(x => x.RefreshTokenHash)
         .HasMaxLength(2048);

      builder.Property(x => x.LastIpAddress)
         .HasMaxLength(512);
      
      builder.HasOne(x => x.Account)
         .WithMany(x => x.Devices)
         .HasForeignKey(x => x.AccountId);
   }
}