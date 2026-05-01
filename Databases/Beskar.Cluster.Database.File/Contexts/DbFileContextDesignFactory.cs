using Beskar.Cluster.Database.Common.Design;
using Beskar.Cluster.Database.Common.Enums;

namespace Beskar.Cluster.Database.File.Contexts;

public sealed class DbFileContextDesignFactory()
   : DbBaseContextDesignTimeFactory<DbFileContext, DbFileContextFactory>(DbContextKind.File)
{
   
}