using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProjectMemberConfiguration : IEntityTypeConfiguration<DbProjectMember>
{
   public static readonly ValueConverter<DbProjectMemberId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbProjectMemberId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbProjectMember> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.HasOne(e => e.Account)
         .WithMany(e => e.ProjectMemberships)
         .HasForeignKey(e => e.AccountId);
      
      builder.HasOne(e => e.Project)
         .WithMany(e => e.Members)
         .HasForeignKey(e => e.ProjectId);
   }
}