using System.Text.Json;
using Beskar.Cluster.Database.Common.Entities;
using Beskar.Cluster.Database.File.Enums;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.File.Entities;

public sealed class DbFileStorageProvider : BaseEntity
{
   public DbFileStorageProviderId Id { get; set; }
   
   public required string DisplayName { get; set; }
   public required FileProviderType Type { get; set; }
   
   public required JsonElement ConfigurationJson { get; set; }
   public bool IsEnabled { get; set; }
   
   public List<DbFileEntry> Entries => field ??= [];
}

[TypeSafeId]
public readonly partial record struct DbFileStorageProviderId(Guid Value);