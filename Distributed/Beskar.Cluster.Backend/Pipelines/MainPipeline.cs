using Beskar.Cluster.Backend.Processors;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Attributes;

namespace Beskar.Cluster.Backend.Pipelines;

[ProcessorPipeline("Main")]
public sealed partial class MainPipeline
{
   [Step(10)]
   public required LogProcessor LogProcessor { get; init; }
   
   [Step(20)]
   public required AuthenticationProcessor AuthenticationProcessor { get; init; }
   
   [Step(30)]
   public required RequestProcessor RequestProcessor { get; init; }
}