using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Translation.Contexts;

public sealed partial class DbTranslationContext(
   DbContextOptions options) 
   : DbBaseContext(options)
{
   public override DbContextKind Kind => DbContextKind.Translation;
}