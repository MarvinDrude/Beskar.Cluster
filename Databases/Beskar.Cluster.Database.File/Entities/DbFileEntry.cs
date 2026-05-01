using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.File.Entities;

public sealed class DbFileEntry : BaseEntity
{
   public DbFileEntryId Id { get; set; }
   
   public required string FileName { get; set; }
   public required string FileExtension { get; set; }
   public required string MimeType { get; set; }
   
   public required string DisplayName { get; set; }
   public required long ByteSize { get; set; }
   
   public required DbFileStorageProviderId StorageProviderId { get; set; }
   public DbFileStorageProvider? StorageProvider { get; set; }
   
   public DbFileEntryFolderId? FolderId { get; set; }
   public DbFileEntryFolder? Folder { get; set; }
   
   public DbFileContent? Content { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbFileEntryId(Guid Value);