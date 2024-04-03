
using Microsoft.Extensions.Options;
using NetDaemon.Client;
using NetDaemon.Client.Settings;
using NetDaemon.HassModel;

namespace DisplayUtil.HomeAssistant;

internal class HassHostedService(
    IOptions<HomeAssistantSettings> options,
    IHomeAssistantRunner haRunner,
    ICacheManager cacheManager,
    ILogger<HassHostedService> logger
) : IHostedService
{
    private CancellationTokenSource _cancellationTokenSource = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var haSettings = options.Value;

        haRunner.OnConnect.SubscribeAsync(OnConnection);

        _ = haRunner.RunAsync(
             haSettings.Host,
             haSettings.Port,
             haSettings.Ssl,
             haSettings.Token,
             TimeSpan.FromSeconds(10),
             _cancellationTokenSource.Token
        );

        return Task.CompletedTask;
    }

    private async Task OnConnection(IHomeAssistantConnection connection)
    {
        logger.LogInformation("Hass Connection initialized");
        await cacheManager.InitializeAsync(_cancellationTokenSource.Token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}