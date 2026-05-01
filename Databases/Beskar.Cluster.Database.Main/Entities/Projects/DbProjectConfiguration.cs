using Beskar.Cluster.Database.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProjectConfiguration : IEntityTypeConfiguration<DbProject>
{
   public static readonly ValueConverter<DbProjectId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbProjectId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbProject> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();
      
      builder.Property(e => e.Name)
         .HasMaxLength(256);

      builder.Property(e => e.Slug)
         .HasMaxLength(256);
      
      builder.Property(e => e.Description)
         .HasMaxLength(1024);
      
      builder.HasOne(e => e.Owner)
         .WithMany(e => e.OwnedProjects)
         .HasForeignKey(e => e.OwnerId);
   }
}