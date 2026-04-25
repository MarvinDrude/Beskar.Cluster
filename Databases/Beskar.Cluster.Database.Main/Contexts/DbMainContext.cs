using Beskar.Cluster.Database.Common.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Main.Contexts;

public sealed partial class DbMainContext(
   DbContextOptions options) 
   : DbBaseContext(options)
{
   
}