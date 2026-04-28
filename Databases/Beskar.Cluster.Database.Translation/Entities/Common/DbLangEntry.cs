using Beskar.Cluster.Database.Common.Entities;

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

public readonly record struct DbLangEntryId(Guid Value);