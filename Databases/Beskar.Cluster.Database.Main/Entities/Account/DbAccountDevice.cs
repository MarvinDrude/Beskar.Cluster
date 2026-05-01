using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.Account;

public sealed class DbAccountDevice : BaseEntity
{
   public DbAccountDeviceId Id { get; set; }
   
   public required string Fingerprint { get; set; }
   public required string DeviceName { get; set; }
   
   public required string RefreshTokenHash { get; set; }
   
   public string? LastIpAddress { get; set; }
   
   public DateTimeOffset LastSignInAt { get; set; }
   public DateTimeOffset LastTwoFactorCheckAt { get; set; }
   
   public required DbAccountId AccountId { get; set; }
   public DbAccount? Account { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbAccountDeviceId(Guid Value);