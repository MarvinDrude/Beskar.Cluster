using System.Text.Json;
using Beskar.CodeGeneration.TypeIdGenerator.Marker.Attributes;

namespace Beskar.Cluster.Database.Main.Entities.System;

public sealed class DbSystemConfigEntry
{
   public DbSystemConfigEntryId Id { get; set; }
   
   public required string Key { get; set; }
   
   public JsonElement Value { get; set; }
}

[TypeSafeId]
public readonly partial record struct DbSystemConfigEntryId(Guid Value);