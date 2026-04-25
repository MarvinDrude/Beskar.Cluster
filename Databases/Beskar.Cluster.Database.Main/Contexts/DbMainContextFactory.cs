using Beskar.Cluster.Database.Common.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Main.Contexts;

public sealed class DbMainContextFactory(
   IServiceProvider serviceProvider,
   IDbContextFactory<DbMainContext> factory)
   : DbBaseContextFactory<DbMainContext>(serviceProvider, factory);