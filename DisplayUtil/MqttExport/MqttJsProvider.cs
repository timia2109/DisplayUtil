using DisplayUtil.EcmaScript.Environment;

namespace DisplayUtil.MqttExport;

public class MqttJsProvider(
    MqttUrlRenderer setter
) : IJsValueProvider
{
    public void OnSetup(IJsExporter exporter)
    {
        exporter.ExposeValue("mqtt", this);
    }

    public void SetMqttTemplate(string templateId)
    {
        _ = SetMqttTemplateAsync(templateId.Trim());
    }

    private async Task SetMqttTemplateAsync(string templateId)
    {
        await setter.GenerateUrlAndPublish(templateId);
    }
}