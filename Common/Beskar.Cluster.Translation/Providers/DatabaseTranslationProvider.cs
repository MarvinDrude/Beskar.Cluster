using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Beskar.Cluster.Database.Translation.Contexts;
using Beskar.CodeGeneration.LanguageGenerator.Marker.Common;
using Beskar.CodeGeneration.LanguageGenerator.Marker.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Translation.Providers;

public sealed class DatabaseTranslationProvider(
   IDbPooledContextFactory<DbTranslationContext> contextFactory,
   IEnumerable<ILanguageDetector> detectors)
   : TranslationBaseProvider(detectors)
{
   private readonly IDbPooledContextFactory<DbTranslationContext> _contextFactory = contextFactory;
   
   public override async ValueTask PopulateCache(CancellationToken cancellationToken = default)
   {
      await using var context = await _contextFactory.CreateAsync(cancellationToken);

      var flat = await context.LangGroups
         .SelectMany(g => g.Keys.SelectMany(k => k.Entries.Select(e => new
         {
            LangCode = e.Language!.TwoLetterCode,
            FullKey = g.Name + "." + k.Key,
            e.Text
         })))
         .ToListAsync(cancellationToken);
      
      var languageGroups = flat
         .GroupBy(x => x.LangCode);

      foreach (var lang in languageGroups)
      {
         var translations = lang.ToDictionary(
            x => x.FullKey,
            x => x.Text
         );

         AddToCache(lang.Key, translations);
      }
   }
}