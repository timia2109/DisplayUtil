using DisplayUtil.EspUtilities;
using DisplayUtil.Template;
using Microsoft.Extensions.Options;

namespace DisplayUtil.MqttExport;

public class MqttUrlRenderer(
    MqttExporter exporter,
    IOptionsSnapshot<MqttSettings> options
)
{
    private Uri GetMqttTemplateUri(
        string providerId
    )
    {
        var settings = options.Value;

        var query = providerId.IndexOf('?');
        var providerPath = query == -1
            ? providerId
            : providerId[0..query];

        var uriBuilder = new UriBuilder(settings.ServerHostName!)
        {
            Path = EspUtilitiesInitExtension.CompressedImageRoute
        };

        uriBuilder.Path = EspUtilitiesInitExtension.CompressedImageRoute
            .Replace("{providerId}", Uri.EscapeDataString(providerPath));

        if (query != -1)
            uriBuilder.Query = providerId[query..];

        return uriBuilder.Uri;
    }

    /// <summary>
    /// Exports the Uri to MQTT
    /// </summary>
    /// <returns>Task</returns>
    public async Task GenerateUrlAndPublish(
        string providerId
    )
    {
        await exporter.PublishUriToMqttAsync(
            GetMqttTemplateUri(providerId)
        );
    }

}