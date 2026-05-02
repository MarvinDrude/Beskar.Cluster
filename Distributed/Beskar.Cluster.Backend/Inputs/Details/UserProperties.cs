using Beskar.Cluster.Database.Main.Entities.Account;

namespace Beskar.Cluster.Backend.Inputs.Details;

public sealed class UserProperties
{
   public string? RawBearerToken { get; set; }
   
   public DbAccountId? AccountId { get; set; }
   
   internal void Clear()
   {
      RawBearerToken = null;
      AccountId = null;
   }
}