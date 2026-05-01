using Beskar.Cluster.Database.Common.Entities;
using Beskar.Cluster.Database.Main.Enums.Account;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountMultiFactor : BaseEntity
{
   public DbAccountMultiFactorId Id { get; set; }
   
   public AccountTwoFactorType Type { get; set; }
   
   public required string Secret { get; set; }
   
   public bool IsVerified { get; set; }
   
   public required DbAccountId AccountId { get; set; }
   public DbAccount? Account { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbAccountMultiFactorId(Guid Value);