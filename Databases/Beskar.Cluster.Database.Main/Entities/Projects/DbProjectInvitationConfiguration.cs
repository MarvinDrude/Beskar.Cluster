using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProjectInvitationConfiguration : IEntityTypeConfiguration<DbProjectInvitation>
{
   public static readonly ValueConverter<DbProjectInvitationId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbProjectInvitationId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbProjectInvitation> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.Property(e => e.Email)
         .HasMaxLength(512);
      
      builder.Property(e => e.TokenHash)
         .HasMaxLength(2048);

      builder.HasOne(e => e.InvitedBy)
         .WithMany()
         .HasForeignKey(e => e.InvitedById);

      builder.HasOne(e => e.Project)
         .WithMany()
         .HasForeignKey(e => e.ProjectId);
      
      builder.HasOne(e => e.Role)
         .WithMany()
         .HasForeignKey(e => e.RoleId);
      
      builder.HasOne(e => e.InvitedAccount)
         .WithMany()
         .HasForeignKey(e => e.InvitedAccountId);

      builder.HasIndex(e => e.TokenHash)
         .IsUnique();
   }
}