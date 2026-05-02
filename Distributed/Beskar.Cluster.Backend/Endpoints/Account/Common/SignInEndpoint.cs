using Beskar.Cluster.Backend.Attributes;
using Beskar.Cluster.Backend.Inputs;
using Beskar.Cluster.Backend.Internal.Enums;

namespace Beskar.Cluster.Backend.Endpoints.Account.Common;

[Endpoint("/account/common/signin", RequestMethod.Post)]
public sealed partial class SignInEndpoint
{
   
   
   public async Task<object> Execute(ProRequest request, CancellationToken ct)
   {
      
   }
}