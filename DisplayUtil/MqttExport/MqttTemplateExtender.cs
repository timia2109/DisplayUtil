using DisplayUtil.Template;
using Scriban.Runtime;

namespace DisplayUtil.MqttExport;

/// <summary>
/// Provides the publish_mqtt_template function to Jobs
/// </summary>
/// <param name="setter"></param>
public class MqttTemplateExtender(
    MqttUrlRenderer setter
) : ITemplateExtender
{
    public void Enrich(
        ScriptObject scriptObject,
        EnrichScope scope)
    {
        if (scope != EnrichScope.Job) return;

        scriptObject.Import("publish_mqtt_template", SetMqttTemplate);
    }

    private void SetMqttTemplate(string templateId)
    {
        _ = SetMqttTemplateAsync(templateId.Trim());
    }

    private async Task SetMqttTemplateAsync(string templateId)
    {
        await setter.GenerateUrlAndPublish(templateId);
    }
}