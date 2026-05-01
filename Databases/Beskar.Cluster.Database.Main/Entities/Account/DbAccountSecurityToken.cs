using Beskar.Cluster.Database.Common.Entities;
using Beskar.Cluster.Database.Main.Enums.Account;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountSecurityToken : BaseEntity
{
   public DbAccountSecurityTokenId Id { get; set; }
   
   public required AccountSecurityTokenType Type { get; set; }
   public required string TokenHash { get; set; }
   
   public required DateTimeOffset ExpiresAt { get; set; }
   public DateTimeOffset? UsedAt { get; set; }
   
   public required DbAccountId AccountId { get; set; }
   public DbAccount? Account { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbAccountSecurityTokenId(Guid Value);