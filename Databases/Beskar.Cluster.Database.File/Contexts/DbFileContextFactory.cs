using Beskar.Cluster.Database.Common.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.File.Contexts;

public sealed class DbFileContextFactory(
   IServiceProvider serviceProvider,
   IDbContextFactory<DbFileContext> factory)
   : DbBaseContextFactory<DbFileContext>(serviceProvider, factory);