using Beskar.Cluster.Database.Common.Design;
using Beskar.Cluster.Database.Common.Enums;

namespace Beskar.Cluster.Database.Main.Contexts;

public sealed class DbMainContextDesignFactory()
   : DbBaseContextDesignTimeFactory<DbMainContext, DbMainContextFactory>(DbContextKind.Main)
{
   
}