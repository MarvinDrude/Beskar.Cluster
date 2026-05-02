using Beskar.Cluster.Backend.Inputs;
using Beskar.Cluster.Translation.Detectors;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Interfaces;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Models;
using Me.Memory.Results;

namespace Beskar.Cluster.Backend.Processors;

public sealed class RequestProcessor()
   : IValueAsyncProcessor<ProRequest, ProRequest>
{
   
   public ValueTask<Result<ProRequest, ProcessorError>> Execute(
      ProcessorContext context, ProRequest input, CancellationToken cancellationToken)
   {
      var (langCode, twoLetter) = DetermineLanguage(input);
      using var langScope = AcceptLanguageDetector.BeginScope(langCode, twoLetter);
      
      
   }

   private static (string langCode, string twoLetter) DetermineLanguage(ProRequest request)
   {
      
   }
}