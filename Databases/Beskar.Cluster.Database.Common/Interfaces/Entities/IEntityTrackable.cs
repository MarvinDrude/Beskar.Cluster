namespace Beskar.Cluster.Database.Common.Interfaces.Entities;

public interface IEntityTrackable
{
   public DateTimeOffset CreatedAt { get; set; }
   
   public DateTimeOffset UpdatedAt { get; set; }
}