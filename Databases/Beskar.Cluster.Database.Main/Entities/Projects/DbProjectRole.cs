using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProjectRole : BaseEntity
{
   public DbProjectRoleId Id { get; set; }
   
   public required string Name { get; set; }
   public required string Description { get; set; }
   
   public bool IsImmutable { get; set; }
   
   public required DbProjectId ProjectId { get; set; }
   public DbProject? Project { get; set; }
   
   public List<DbProjectRolePermission> Permissions => field ??= [];
}

[TypeSafeId]
public readonly partial record struct DbProjectRoleId(Guid Value);