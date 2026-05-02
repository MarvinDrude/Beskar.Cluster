using Beskar.Cluster.Backend.Inputs;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Interfaces;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Models;
using Me.Memory.Results;

namespace Beskar.Cluster.Backend.Processors;

public sealed class AuthenticationProcessor : IAsyncProcessor<ProRequest, ProRequest>
{
   public Task<Result<ProRequest, ProcessorError>> Execute(
      ProcessorContext context, ProRequest input, CancellationToken cancellationToken)
   {
      
   }
}