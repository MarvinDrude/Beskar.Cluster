namespace Beskar.Cluster.Backend.Inputs.Details;

public sealed class UserProperties
{
   public string? RawBearerToken { get; set; }
   
   internal void Clear()
   {
      RawBearerToken = null;
   }
}