using Beskar.Cluster.Configuration.Models;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Microsoft.Extensions.Options;

namespace Beskar.Cluster.Database.Common.Contexts;

public sealed class DbConnectionStringProvider(IOptionsMonitor<MainOptions> optionsMonitor)
   : IDbConnectionStringProvider
{
   private readonly IOptionsMonitor<MainOptions> _optionsMonitor = optionsMonitor;
   private MainOptions Options => _optionsMonitor.CurrentValue;
   
   public ValueTask<string> GetConnectionString(DbContextKind kind, CancellationToken ct = default)
   {
      return new ValueTask<string>(kind switch
      {
         DbContextKind.Main => Options.MainDatabaseConnectionString,
         DbContextKind.Translation => Options.TranslationConnectionString,
         DbContextKind.Unknown => throw new InvalidOperationException($"Invalid DbContextKind: {kind}"),
         _ => throw new InvalidOperationException($"Invalid DbContextKind: {kind}")
      });
   }
}