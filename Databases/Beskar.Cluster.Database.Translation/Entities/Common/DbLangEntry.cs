using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public class DbLangEntry : BaseEntity
{
   public DbLangEntryId Id { get; set; }
   
   public string Text { get; set; } = string.Empty;
   
   public DbLangKeyId LangKeyId { get; set; }
   public DbLangKey? LangKey { get; set; }
   
   public DbLanguageId LanguageId { get; set; }
   public DbLanguage? Language { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbLangEntryId(Guid Value);