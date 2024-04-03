using System.Diagnostics;
using DisplayUtil.EspUtilities;
using DisplayUtil.Scenes;
using MQTTnet.Client;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Responsible to export the image to Mqtt.
/// Scoped
/// </summary>
public class MqttExporter(
    EspImageProvider espImageProvider,
    ExportingMqttClient exportingMqttClient
)
{
    public async Task ExportScreenToMqtt(string providerId)
    {
        var (data, _) = await espImageProvider.GetAsRunLengthAsync(providerId);
        await exportingMqttClient.SendAsync(data);
    }
}