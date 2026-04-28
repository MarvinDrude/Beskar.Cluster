using Beskar.Cluster.Database.Common.Entities;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLangGroup : BaseEntity
{
   public DbLangGroupId Id { get; set; }
   
   public required string Name { get; set; }
}

public readonly record struct DbLangGroupId(Guid Value);