using Beskar.Cluster.Database.Common.Contexts;
using Beskar.Cluster.Database.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.File.Contexts;

public sealed partial class DbFileContext(
   DbContextOptions options) 
   : DbBaseContext(options)
{
   public override DbContextKind Kind => DbContextKind.File;
}