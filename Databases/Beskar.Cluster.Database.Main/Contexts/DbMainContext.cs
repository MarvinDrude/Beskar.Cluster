using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Main.Contexts;

public sealed partial class DbMainContext(
   DbContextOptions options) 
   : DbBaseContext(options)
{
   public override DbContextKind Kind => DbContextKind.Main;
}