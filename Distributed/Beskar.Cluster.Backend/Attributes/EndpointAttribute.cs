using Beskar.Cluster.Backend.Internal.Enums;

namespace Beskar.Cluster.Backend.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EndpointAttribute(
   string path,
   RequestMethod method) 
   : Attribute
{
   public string Path { get; init; } = path;
   
   public RequestMethod Method { get; init; } = method;
}