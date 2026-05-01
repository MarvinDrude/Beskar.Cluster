using Beskar.Cluster.Database.Common.Entities;
using Beskar.Cluster.Database.Main.Enums.Account;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccount : BaseEntity
{
   public DbAccountId Id { get; set; }
   
   public required string Email { get; set; }
   public string? PasswordHash { get; set; }
   
   public required bool IsEmailVerified { get; set; }

   public AccountStatus Status { get; set; } = AccountStatus.Pending;
   
   public List<DbAccountBackupCode> BackupCodes => field ??= [];
   
   public List<DbAccountDevice> Devices => field ??= [];
   
   public List<DbAccountExternal> Externals => field ??= [];
   
   public List<DbAccountMultiFactor> MultiFactors => field ??= [];
   
   public List<DbAccountSecurityToken> SecurityTokens => field ??= [];
}

[TypeSafeId]
public readonly partial record struct DbAccountId(Guid Value);