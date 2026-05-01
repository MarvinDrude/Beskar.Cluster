using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProjectRolePermission : BaseEntity
{
   public DbProjectRolePermissionId Id { get; set; }
   
   public required string PermissionKey { get; set; }
   
   public required DbProjectRoleId ProjectRoleId { get; set; }
   public DbProjectRole? ProjectRole { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbProjectRolePermissionId(Guid Value);