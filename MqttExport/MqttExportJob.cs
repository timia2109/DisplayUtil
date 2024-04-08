using Microsoft.Extensions.Options;
using NetDaemon.HassModel;

namespace DisplayUtil.MqttExport;

public class MqttExportJob(
    IServiceScopeFactory scopeFactory,
    IOptions<MqttSettings> optionsSnapshot
) : IHostedService
{
    private static readonly TimeSpan InitTimeout = TimeSpan.FromSeconds(5);
    private CancellationTokenSource? _cancellationTokenSource;

    private async Task RunAsync(CancellationToken cancellation)
    {
        await Task.Delay(InitTimeout);

        while (!cancellation.IsCancellationRequested)
        {
            await using (var scope = scopeFactory.CreateAsyncScope())
            {
                var renderer = scope.ServiceProvider
                    .GetRequiredService<MqttUrlRenderer>();
                await renderer.RenderUrlAndPublish();
            }

            await Task.Delay(optionsSnapshot.Value.RefreshInterval!.Value,
                cancellation);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _ = RunAsync(_cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }
}