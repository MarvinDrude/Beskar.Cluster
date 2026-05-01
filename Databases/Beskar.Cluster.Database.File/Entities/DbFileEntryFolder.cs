using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.File.Entities;

public sealed class DbFileEntryFolder : BaseEntity
{
   public DbFileEntryFolderId Id { get; set; }
   
   public required string Name { get; set; }
   
   public DbFileEntryFolderId? ParentFolderId { get; set; }
   public DbFileEntryFolder? ParentFolder { get; set; }
   
   public List<DbFileEntryFolder> SubFolders => field ??= [];
   public List<DbFileEntry> Files => field ??= [];
}

[TypeSafeId]
public readonly partial record struct DbFileEntryFolderId(Guid Value);