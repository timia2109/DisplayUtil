using DisplayUtil.EspUtilities;
using DisplayUtil.Template;
using Microsoft.Extensions.Options;

namespace DisplayUtil.MqttExport;

public class MqttUrlRenderer(
    TemplateRenderer renderer,
    MqttExporter exporter,
    IOptionsSnapshot<MqttSettings> options
)
{
    public async Task<Uri> GetMqttTemplateUriAsync()
    {
        var providerId = await GetMqttTemplateAsync();

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

    public async Task<string> GetMqttTemplateAsync()
    {
        var template = options.Value.ScreenDetectTemplate!;

        var result = await renderer.RenderAsync(template,
            EnrichScope.TemplateProvider);

        return result.Trim();
    }

    /// <summary>
    /// Exports the Uri to MQTT
    /// </summary>
    /// <returns>Task</returns>
    public async Task RenderUrlAndPublish()
    {
        await exporter.PublishUriToMqttAsync(await GetMqttTemplateUriAsync());
    }

}