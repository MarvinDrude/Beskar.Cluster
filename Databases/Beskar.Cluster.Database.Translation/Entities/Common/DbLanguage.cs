using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLanguage : BaseEntity
{
   public DbLanguageId Id { get; set; }
   
   /// <summary>
   /// Example: English, Russian, German, French, ...
   /// </summary>
   public required string DisplayName { get; set; }
   
   /// <summary>
   /// Example: en, ru, de, fr, ...
   /// </summary>
   public required string TwoLetterCode { get; set; }
   
   /// <summary>
   /// Example: en-US, ru-RU, de-DE, fr-FR, ...
   /// </summary>
   public required string Name { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbLanguageId(Guid Value);