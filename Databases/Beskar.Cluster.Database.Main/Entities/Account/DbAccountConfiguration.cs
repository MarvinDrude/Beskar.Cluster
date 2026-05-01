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
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql("uuidv7()")
         .ValueGeneratedOnAdd();

      builder.Property(e => e.Email)
         .HasMaxLength(512);
      
      builder.Property(e => e.PasswordHash)
         .HasMaxLength(2048);

      builder.HasIndex(e => e.Email)
         .IsUnique()
         .HasFilter($@"""{nameof(DbAccount.IsDeleted)}"" = FALSE");
   }
}