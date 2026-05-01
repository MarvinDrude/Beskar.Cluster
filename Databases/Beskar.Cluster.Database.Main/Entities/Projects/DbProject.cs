using Beskar.Cluster.Database.Common.Entities;
using Beskar.Cluster.Database.Main.Entities.Account;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProject : BaseEntity
{
   public DbProjectId Id { get; set; }
   
   public required string Name { get; set; }
   public required string Slug { get; set; }
   
   public required string Description { get; set; }
   
   public required DbAccountId OwnerId { get; set; }
   public DbAccount? Owner { get; set; }
   
   public List<DbProjectMember> Members => field ??= [];
   public List<DbProjectInvitation> Invitations => field ??= [];
   public List<DbProjectRole> Roles => field ??= [];
}

[TypeSafeId]
public readonly partial record struct DbProjectId(Guid Value);