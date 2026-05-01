using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProjectRolePermissionConfiguration : IEntityTypeConfiguration<DbProjectRolePermission>
{
   public static readonly ValueConverter<DbProjectRolePermissionId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbProjectRolePermissionId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbProjectRolePermission> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();
      
      builder.Property(e => e.PermissionKey)
         .HasMaxLength(256);
      
      builder.HasOne(e => e.ProjectRole)
         .WithMany(e => e.Permissions)
         .HasForeignKey(e => e.ProjectRoleId);
   }
}