using Beskar.Cluster.Database.Common.Entities;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLangKey : BaseEntity
{
   public DbLangKeyId Id { get; set; }
   
   public required string Key { get; set; }
   
   public DbLangGroupId LangGroupId { get; set; }
   public DbLangGroup? LangGroup { get; set; }
   
   public List<DbLangEntry> Entries => field ??= [];
}

public readonly record struct DbLangKeyId(Guid Value);