using Beskar.Cluster.Database.Common.Interfaces.Entities;

namespace Beskar.Cluster.Database.Common.Entities;

public abstract class BaseEntity : IEntityTrackable, IEntitySoftDeletable
{
   public DateTimeOffset CreatedAt { get; set; }
   
   public DateTimeOffset UpdatedAt { get; set; }
   
   public bool IsDeleted { get; set; }
   public DateTimeOffset? DeletedAt { get; set; }
}