using DisplayUtil.Template;
using Microsoft.Extensions.Options;

namespace DisplayUtil.MqttExport;

public class MqttUrlRenderer(
    TemplateRenderer renderer,
    MqttExporter exporter,
    IOptionsSnapshot<MqttSettings> options
)
{

    /// <summary>
    /// Exports the Uri to MQTT
    /// </summary>
    /// <returns>Task</returns>
    public async Task RenderUrlAndPublish()
    {
        var template = options.Value.ScreenDetectTemplate!;

        var result = await renderer.RenderAsync(template,
            EnrichScope.TemplateProvider);

        await exporter.ExportUriToMqtt(result);
    }

}