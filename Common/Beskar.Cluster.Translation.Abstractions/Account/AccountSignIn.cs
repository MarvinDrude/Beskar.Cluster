using Beskar.CodeGeneration.LanguageGenerator.Marker.Attributes;

namespace Beskar.Cluster.Translation.Abstractions.Account;

[TranslationGroup]
public enum AccountSignIn
{
   [TranslationKey(defaultValue: "Sign in")]
   Title = 1,
}