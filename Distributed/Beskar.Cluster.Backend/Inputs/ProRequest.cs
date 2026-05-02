using Beskar.Cluster.Backend.Inputs.Details;

namespace Beskar.Cluster.Backend.Inputs;

public sealed class ProRequest
{
   public UserProperties UserProperties { get; set; } = new();

   internal void Clear()
   {
      UserProperties.Clear();
   }
}