using Beskar.Cluster.Database.Common.Entities;
using Beskar.Cluster.Database.Main.Entities.Account;

namespace Beskar.Cluster.Database.Main.Entities.Projects;

public sealed class DbProjectInvitation : BaseEntity
{
   public DbProjectInvitationId Id { get; set; }
   
   public required string Email { get; set; }
   public required string TokenHash { get; set; }
   
   public DateTimeOffset? ExpiresAt { get; set; }
   public DateTimeOffset? AcceptedAt { get; set; }
   
   public DbAccountId? InvitedAccountId { get; set; }
   public DbAccount? InvitedAccount { get; set; }
   
   public required DbProjectId ProjectId { get; set; }
   public DbProject? Project { get; set; }
   
   public required DbAccountId InvitedById { get; set; }
   public DbAccount? InvitedBy { get; set; }
   
   public required DbProjectRoleId RoleId { get; set; }
   public DbProjectRole? Role { get; set; }
}

public readonly partial record struct DbProjectInvitationId(Guid Value);