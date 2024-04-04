using Microsoft.Extensions.Options;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Responsible to export the new URL to MQTT
/// </summary>
public partial class ExportingMqttClient(
    IMqttClient client,
    MqttClientOptions clientOptions,
    IOptions<MqttSettings> settings,
    ILogger<ExportingMqttClient> logger
)
{

    private readonly ILogger _logger = logger;

    private async Task EnsureConnectedAsync()
    {
        if (!client.IsConnected)
        {
            LogConnecting();
            await client.ConnectAsync(clientOptions);
        }
    }

    public async Task SendAsync(string payload)
    {
        await EnsureConnectedAsync();

        await client.PublishStringAsync(
            settings.Value.Topic
                ?? throw new ArgumentNullException(nameof(settings.Value.Topic)),
            payload,
            MqttQualityOfServiceLevel.AtLeastOnce,
            true
        );
    }

    [LoggerMessage(LogLevel.Information, "Connecting to Mqtt Server")]
    private partial void LogConnecting();
}