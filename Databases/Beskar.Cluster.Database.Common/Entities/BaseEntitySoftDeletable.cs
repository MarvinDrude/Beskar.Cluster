using Beskar.Cluster.Database.Common.Interfaces.Entities;

namespace Beskar.Cluster.Database.Common.Entities;

public abstract class BaseEntitySoftDeletable : IEntitySoftDeletable
{
   public bool IsDeleted { get; set; }
   
   public DateTimeOffset? DeletedAt { get; set; }
}