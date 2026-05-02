using System.Security.Claims;

namespace Beskar.Cluster.Jwt.Extensions;

public static class ClaimsPrincipalExtensions
{
   extension(ClaimsPrincipal principal)
   {
      public string? RawAccountIdentifier => principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
   }
}