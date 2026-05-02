using Beskar.Cluster.Backend.Inputs.Details;
using Beskar.Cluster.Backend.Internal.Enums;

namespace Beskar.Cluster.Backend.Inputs;

public sealed class ProRequest
{
   public string Path { get; set; } = string.Empty;
   public RequestMethod Method { get; set; } = RequestMethod.Get;
   
   public string? HeaderAcceptLanguage { get; set; }
   
   public UserProperties UserProperties { get; set; } = new();
   
   internal void Clear()
   {
      UserProperties.Clear();
   }
}