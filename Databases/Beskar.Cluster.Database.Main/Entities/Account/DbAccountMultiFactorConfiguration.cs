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
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql("uuidv7()")
         .ValueGeneratedOnAdd();
      
      builder.Property(e => e.Secret)
         .HasMaxLength(2048);
      
      builder.HasOne(e => e.Account)
         .WithMany(e => e.MultiFactors)
         .HasForeignKey(e => e.AccountId);
   }
}