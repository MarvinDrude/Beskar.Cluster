using Beskar.Cluster.Database.Common.Interfaces.Entities;

namespace Beskar.Cluster.Database.Common.Entities;

public abstract class BaseEntityTrackable : IEntityTrackable
{
   public DateTimeOffset CreatedAt { get; set; }
   
   public DateTimeOffset UpdatedAt { get; set; }
}