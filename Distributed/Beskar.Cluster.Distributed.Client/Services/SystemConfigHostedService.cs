using Beskar.Cluster.Distributed.Client.Caches;
using Beskar.Cluster.Distributed.Client.Constants;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Beskar.Cluster.Distributed.Client.Services;

public sealed class SystemConfigHostedService(
   IConnectionMultiplexer connectionMultiplexer,
   LocalSystemConfigCache localSystemConfigCache,
   IServiceProvider serviceProvider)
   : IHostedService, IAsyncDisposable
{
   private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
   private readonly LocalSystemConfigCache _localSystemConfigCache = localSystemConfigCache;
   private readonly IServiceProvider _serviceProvider = serviceProvider;
   
   private CancellationTokenSource? _cts;
   private Task? _runningTask;
   
   public Task StartAsync(CancellationToken cancellationToken)
   {
      _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
      _runningTask = RunAsync(_cts.Token);

      return Task.CompletedTask;
   }

   public Task StopAsync(CancellationToken cancellationToken)
   {
      _cts?.Cancel();
      return Task.CompletedTask;
   }

   private async Task RunAsync(CancellationToken ct)
   {
      await _localSystemConfigCache.Refresh(ct);

      var db = _connectionMultiplexer.GetSubscriber();
      var channel = await db.SubscribeAsync(DistributedChannels.RefreshSystemConfigChannel);

      try
      {
         await foreach (var _ in channel.WithCancellation(ct))
         {
            await _localSystemConfigCache.Refresh(ct);
         }
      }
      catch (OperationCanceledException)
      {
         await db.UnsubscribeAsync(DistributedChannels.RefreshSystemConfigChannel);
      }
   }

   public async ValueTask DisposeAsync()
   {
      if (_cts is not null)
      {
         await _cts.CancelAsync();
      }
      
      if (_runningTask is not null)
      {
         await _runningTask;
      }
      
      _cts = null;
      _runningTask = null;
   }
}