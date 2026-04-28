using Beskar.Cluster.Database.Common.Design;
using Beskar.Cluster.Database.Common.Enums;

namespace Beskar.Cluster.Database.Translation.Contexts;

public sealed class DbTranslationDesignFactory()
   : DbBaseContextDesignTimeFactory<DbTranslationContext, DbTranslationContextFactory>(DbContextKind.Translation)
{
   
}