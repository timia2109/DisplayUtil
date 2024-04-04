using Microsoft.Extensions.Options;

namespace DisplayUtil.MqttExport;

public class MqttExportJob(
    IServiceScopeFactory scopeFactory,
    IOptions<MqttSettings> optionsSnapshot
) : IHostedService
{
    private CancellationTokenSource? _cancellationTokenSource;

    private async Task RunAsync(CancellationToken cancellation)
    {
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