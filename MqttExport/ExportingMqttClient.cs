using Microsoft.Extensions.Options;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Responsible to export the Image to Mqtt
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

    public async Task SendAsync(byte[] data)
    {
        await EnsureConnectedAsync();

        await client.PublishBinaryAsync(
            settings.Value.Topic
                ?? throw new ArgumentNullException(nameof(settings.Value.Topic)),
            data,
            MqttQualityOfServiceLevel.AtLeastOnce
        );
    }

    [LoggerMessage(LogLevel.Information, "Connecting to Mqtt Server")]
    private partial void LogConnecting();
}