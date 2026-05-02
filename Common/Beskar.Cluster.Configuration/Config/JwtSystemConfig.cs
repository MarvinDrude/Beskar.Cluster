namespace Beskar.Cluster.Configuration.Config;

public class JwtSystemConfig
   : SystemConfig<JwtOptions>;

public sealed class JwtOptions
{
   public const int KeyLength = 256;
   
   public required string Issuer { get; set; }
   
   public required byte[] KeyV1 { get; set; }
   public required byte[] KeyV2 { get; set; }
   
   public required bool IsV2Enabled { get; set; }
   
   public required int RefreshExpirationInDays { get; set; }
   public required int ExpirationInMinutes { get; set; }
   
   public required DateTimeOffset SwitchedAt { get; set; }
   public required TimeSpan SwapKeysInterval { get; set; }
}