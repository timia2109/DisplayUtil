using DisplayUtil.Utils;
using Microsoft.Extensions.Options;
using NetDaemon.HassModel;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Job to publish the URI to MQTT
/// </summary>
public class MqttExportJob(
    IServiceScopeFactory scopeFactory,
    ILogger<MqttExportJob> logger,
    IOptions<MqttSettings> options
) : TimedScopedService(scopeFactory, logger)
{
    protected override TimeSpan InitTimeout => TimeSpan.FromSeconds(5);
    protected override TimeSpan Delay => options.Value.RefreshInterval!.Value;

    protected override async Task TriggerAsync(IServiceProvider serviceProvider)
    {
        var renderer = serviceProvider.GetRequiredService<MqttUrlRenderer>();
        await renderer.RenderUrlAndPublish();
    }
}