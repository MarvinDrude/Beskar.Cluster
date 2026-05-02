using System.Diagnostics;
using Beskar.Cluster.Backend.Inputs;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Attributes;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Interfaces;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Models;
using Me.Memory.Results;
using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Backend.Processors;

[Processor]
public sealed partial class LogProcessor(ILogger<LogProcessor> logger)
   : ISyncProcessor<ProRequest, ProRequest>, ISyncPostProcessor
{
   private readonly ILogger<LogProcessor> _logger = logger;
   private long _runtimeStart;
   
   public Result<ProRequest, ProcessorError> Execute(
      ProcessorContext context, ProRequest input, CancellationToken cancellationToken)
   {
      _runtimeStart = Stopwatch.GetTimestamp();
      LogStart(context.PipelineName);

      return input;
   }

   public ProcessorError? Post(ProcessorContext context, CancellationToken cancellationToken)
   {
      var duration = Stopwatch.GetElapsedTime(_runtimeStart);
      LogStop(context.PipelineName, duration);

      return null;
   }
}