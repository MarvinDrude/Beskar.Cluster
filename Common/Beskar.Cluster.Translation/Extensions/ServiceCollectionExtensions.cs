using Beskar.Cluster.Translation.Detectors;
using Beskar.Cluster.Translation.Providers;
using Beskar.CodeGeneration.LanguageGenerator.Marker.Interfaces;
using Beskar.Languages;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Translation.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection services)
   {
      public IServiceCollection AddBeskarClusterTranslationServices()
      {
         return services.AddSingleton<ITranslationProvider, DatabaseTranslationProvider>()
            .AddSingleton<ILanguageDetector, AcceptLanguageDetector>()
            .AddSingleton<TranslationFacade>();
      }
   }
}