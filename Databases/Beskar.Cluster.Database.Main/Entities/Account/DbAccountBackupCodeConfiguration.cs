using Beskar.Cluster.Database.Common.Constants;
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
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();
      
      builder.Property(x => x.CodeHash)
         .HasMaxLength(2048);
      
      builder.Property(x => x.UsedByIpAddress)
         .HasMaxLength(512);

      builder.HasOne(x => x.Account)
         .WithMany(x => x.BackupCodes)
         .HasForeignKey(x => x.AccountId);
   }
}