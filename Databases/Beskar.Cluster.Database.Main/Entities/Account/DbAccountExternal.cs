using Beskar.Cluster.Database.Common.Entities;
using Beskar.Cluster.Database.Main.Enums.Account;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountExternal : BaseEntity
{
   public DbAccountExternalId Id { get; set; }
   
   public AccountExternalType Type { get; set; }
   
   public required string ProviderKey { get; set; }
   public required string UserId { get; set; }
   
   public required DbAccountId AccountId { get; set; }
   public DbAccount? Account { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbAccountExternalId(Guid Value);