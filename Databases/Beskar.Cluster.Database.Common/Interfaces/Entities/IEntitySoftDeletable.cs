namespace Beskar.Cluster.Database.Common.Interfaces.Entities;

public interface IEntitySoftDeletable
{
   public bool IsDeleted { get; set; }
   
   public DateTimeOffset? DeletedAt { get; set; }
}