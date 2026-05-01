using Beskar.Cluster.Database.Common.Entities;
using Beskar.Cluster.Database.Main.Entities.Account;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProjectMember : BaseEntity
{
   public DbProjectMemberId Id { get; set; }
   
   public DateTimeOffset JoinedAt { get; set; }
   
   public required DbAccountId AccountId { get; set; }
   public DbAccount? Account { get; set; }
   
   public required DbProjectId ProjectId { get; set; }
   public DbProject? Project { get; set; }
   
   public List<DbProjectRole> Roles => field ??= [];
}

[TypeSafeId]
public readonly partial record struct DbProjectMemberId(Guid Value);