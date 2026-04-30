using Beskar.Cluster.Database.Translation.Contexts;
using Beskar.Cluster.Database.Translation.Entities.Common;
using Beskar.Languages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Beskar.Cluster.Database.Update.Seed;

public static class TranslationSeedRunner
{
   public static async Task Seed(AsyncServiceScope scope, DbTranslationContext context, CancellationToken ct = default)
   {
      var languages = await GetOrCreateLanguages(context, ct);
      var allKeys = LangKey.AllKeys.ToHashSet();
      
      var flat = await context.LangGroups
         .SelectMany(g => g.Keys.SelectMany(k => k.Entries.Select(e => new
         {
            LangCode = e.Language!.TwoLetterCode,
            FullKey = g.Name + "." + k.Key,
            Key = k.Key,
            KeyId = k.Id,
            Group = g.Name,
            GroupId = g.Id,
            e.Text
         })))
         .ToListAsync(ct);
      
      var groups = flat.GroupBy(x => x.Group)
         .ToDictionary(g => g.Key, g => g.First().GroupId);
      var keys = flat.GroupBy(x => x.FullKey)
         .ToDictionary(
            group => group.Key, 
            group => group.First().KeyId
         );

      foreach (var lang in languages)
      {
         var translations = flat.Where(f => f.LangCode == lang.TwoLetterCode);
         
         foreach (var missingKey in allKeys.Except(translations.Select(t => t.FullKey)))
         {
            var defaultValue = LangKey.GetDefaultValue(missingKey);
            
            var firstIndex = missingKey.IndexOf('.');
            var groupName = missingKey[..firstIndex];
            var key = missingKey[(firstIndex + 1)..];
            
            if (!groups.TryGetValue(groupName, out var groupId))
            {
               var insertGroup = new DbLangGroup()
               {
                  Name = groupName,
               };
               
               context.LangGroups.Add(insertGroup);
               await context.SaveChangesAsync(ct);
               
               groupId = insertGroup.Id;
               groups[groupName] = groupId;
            }

            if (!keys.TryGetValue(missingKey, out var keyId))
            {
               var insertKey = new DbLangKey()
               {
                  Key = key,
                  LangGroupId = groupId,
               };
               
               context.LangKeys.Add(insertKey);
               await context.SaveChangesAsync(ct);
               
               keyId = insertKey.Id;
               keys[missingKey] = keyId;
            }

            var insert = new DbLangEntry()
            {
               Text = defaultValue,
               LangKeyId = keyId,
               LanguageId = lang.Id,
            };
            
            context.LangEntries.Add(insert);
         }
      }
      
      await context.SaveChangesAsync(ct);
   }

   private static async Task<List<DbLanguage>> GetOrCreateLanguages(DbTranslationContext context, CancellationToken ct = default)
   {
      var languages = await context.Languages.ToListAsync(ct);
      if (languages.Count > 0)
      {
         return languages;
      }

      List<DbLanguage> defaults = [
         new ()
         {
            Name = "en-US",
            TwoLetterCode = "en",
            DisplayName = "English",
         },
         new ()
         {
            Name = "de-DE",
            TwoLetterCode = "de",
            DisplayName = "German",
         }
      ];
      await context.AddRangeAsync(defaults, ct);
      await context.SaveChangesAsync(ct);
      
      return defaults;
   }
}