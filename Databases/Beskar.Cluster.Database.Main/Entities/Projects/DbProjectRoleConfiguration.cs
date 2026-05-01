using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProjectRoleConfiguration : IEntityTypeConfiguration<DbProjectRole>
{
   public static readonly ValueConverter<DbProjectRoleId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbProjectRoleId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbProjectRole> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.Property(e => e.Name)
         .HasMaxLength(256);

      builder.Property(e => e.Description)
         .HasMaxLength(2048);

      builder.HasOne(e => e.Project)
         .WithMany(e => e.Roles)
         .HasForeignKey(e => e.ProjectId);
   }
}