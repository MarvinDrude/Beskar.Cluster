using Beskar.Cluster.Database.Common.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Translation.Contexts;

public sealed class DbTranslationContextFactory(
   IServiceProvider serviceProvider,
   IDbContextFactory<DbTranslationContext> factory)
   : DbBaseContextFactory<DbTranslationContext>(serviceProvider, factory);