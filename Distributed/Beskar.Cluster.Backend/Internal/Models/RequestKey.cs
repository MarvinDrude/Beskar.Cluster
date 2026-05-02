using Beskar.Cluster.Backend.Internal.Enums;

namespace Beskar.Cluster.Backend.Internal.Models;

public readonly record struct RequestKey(RequestMethod Method, string Path);