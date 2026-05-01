using Beskar.Cluster.Database.Common.Constants;
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
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();
      
      builder.Property(x => x.ProviderKey)
         .HasMaxLength(256);

      builder.Property(x => x.UserId)
         .HasMaxLength(512);
      
      builder.HasOne(x => x.Account)
         .WithMany(x => x.Externals)
         .HasForeignKey(x => x.AccountId);
   }
}