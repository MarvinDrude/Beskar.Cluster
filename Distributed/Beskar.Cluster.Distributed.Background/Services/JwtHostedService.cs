using Beskar.Cluster.Configuration.Config;
using Beskar.Cluster.Configuration.Constants;
using Beskar.Cluster.Distributed.Client.Interfaces;
using Beskar.Cluster.Utilities.Randoms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Beskar.Cluster.Distributed.Background.Services;

public sealed partial class JwtHostedService(
   ILogger<JwtHostedService> logger,
   IServiceProvider serviceProvider)
   : IHostedService, IAsyncDisposable
{
   private readonly IServiceProvider _serviceProvider = serviceProvider;
   private readonly ILogger<JwtHostedService> _logger = logger;
   
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
      while (!ct.IsCancellationRequested)
      {
         try
         {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var config = scope.ServiceProvider.GetRequiredService<ISystemConfigClient>();

            var jwtOptionsWrapper = config.GetValue<JwtSystemConfig>(ConfigurationKeys.JwtOptions)
               ?? throw new InvalidOperationException("Jwt options not found");
            var jwtOptions = jwtOptionsWrapper.Value;
            var refreshInterval = jwtOptions.SwapKeysInterval;

            if (jwtOptions.IsV2Enabled)
            {
               jwtOptions.KeyV1 = RandomUtils.GenerateRandomBytes(JwtOptions.KeyLength);
               jwtOptions.IsV2Enabled = false;
            }
            else
            {
               jwtOptions.KeyV2 = RandomUtils.GenerateRandomBytes(JwtOptions.KeyLength);
               jwtOptions.IsV2Enabled = true;
            }
            
            await config.SetValue(ConfigurationKeys.JwtOptions, jwtOptionsWrapper, ct);
            await Task.Delay(refreshInterval, ct);
         }
         catch (OperationCanceledException)
         {
            // expected
         }
         catch (Exception err)
         {
            // ignored
            LogUnexpectedError(err);
            await Task.Delay(TimeSpan.FromSeconds(60), ct);
         }
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