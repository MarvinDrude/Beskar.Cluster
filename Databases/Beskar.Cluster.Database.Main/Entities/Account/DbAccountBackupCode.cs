using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountBackupCode : BaseEntity
{
   public DbAccountBackupCodeId Id { get; set; }
   
   public required string CodeHash { get; set; }
   
   public DateTimeOffset? UsedAt { get; set; }
   public string? UsedByIpAddress { get; set; }
   
   public required DbAccountId AccountId { get; set; }
   public DbAccount? Account { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbAccountBackupCodeId(Guid Value);