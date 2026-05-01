using Beskar.Cluster.Database.Common.Entities;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.File.Entities;

/// <summary>
/// Only for tiny files
/// </summary>
public sealed class DbFileContent : BaseEntity
{
   public DbFileContentId Id { get; set; }
   
   public required byte[] Bytes { get; set; }
   
   public required DbFileEntryId FileEntryId { get; set; }
   public DbFileEntry? FileEntry { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbFileContentId(Guid Value);