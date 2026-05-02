using Beskar.CodeGeneration.LanguageGenerator.Marker.Interfaces;

namespace Beskar.Cluster.Translation.Detectors;

public sealed class AcceptLanguageDetector : ILanguageDetector
{
   private static readonly AsyncLocal<AcceptLanguageScope?> _currentScope = new ();

   private static AcceptLanguageScope? CurrentScope
   {
      get => _currentScope.Value;
      set => _currentScope.Value = value;
   }

   public int Priority => 1000;
   
   public string? GetLanguageCode(bool isTwoLetterCode)
   {
      var current = CurrentScope;
      if (current is null) return null;

      return isTwoLetterCode ? current.TwoLetterCode : current.LanguageCode;
   }

   public static IDisposable BeginScope(string languageCode, string twoLetterCode)
   {
      var previous = CurrentScope;
      CurrentScope = new AcceptLanguageScope(languageCode, twoLetterCode);
      
      return new AcceptLanguageDisposer(previous);
   }

   public static IDisposable BeginScope(string headerText)
   {
      if (string.IsNullOrWhiteSpace(headerText))
      {
         return BeginScope("en", "en-US");
      }
      
      var span = headerText.AsSpan();
      var bestCode = ReadOnlySpan<char>.Empty;
      var bestScore = -1f;

      while (span.Length > 0)
      {
         var commaIndex = span.IndexOf(',');
         var segment = commaIndex < 0 ? span : span[..commaIndex];
         
         var semiIndex = segment.IndexOf(';');
         ReadOnlySpan<char> code;
         var quality = 1.0f;
         
         if (semiIndex < 0)
         {
            code = segment.Trim();
         }
         else
         {
            code = segment[..semiIndex].Trim();
            var qPart = segment[(semiIndex + 1)..].Trim();
            
            if (qPart.StartsWith("q=", StringComparison.OrdinalIgnoreCase))
            {
               _ = float.TryParse(qPart[2..], out quality);
            }
         }
         
         if (quality >= 1.0f)
         {
            return CreateScopeFromTag(code.ToString());
         }
         
         if (quality > bestScore)
         {
            bestScore = quality;
            bestCode = code;
         }
         
         span = commaIndex < 0 ? ReadOnlySpan<char>.Empty : span[(commaIndex + 1)..];
      }

      return bestCode.IsEmpty 
         ? BeginScope("en", "en-US") 
         : CreateScopeFromTag(bestCode);
   }
   
   private static IDisposable CreateScopeFromTag(ReadOnlySpan<char> fullTag)
   {
      var dashIndex = fullTag.IndexOfAny('-', '_');
      var languageCode = dashIndex < 0 ? fullTag : fullTag[..dashIndex];

      return BeginScope(languageCode.ToString(), fullTag.ToString());
   }
   
   private sealed class AcceptLanguageDisposer(AcceptLanguageScope? previous) : IDisposable
   {
      public void Dispose()
      {
         CurrentScope = previous;
      }
   }

   private sealed record AcceptLanguageScope(string LanguageCode, string TwoLetterCode);
}