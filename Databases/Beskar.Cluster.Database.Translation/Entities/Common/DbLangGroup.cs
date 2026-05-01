using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Translation.Entities.Common;

public sealed class DbLangGroup : BaseEntity
{
   public DbLangGroupId Id { get; set; }
   
   public required string Name { get; set; }

   public List<DbLangKey> Keys => field ??= [];
}

[TypeSafeId]
public readonly partial record struct DbLangGroupId(Guid Value);