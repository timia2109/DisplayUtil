using DisplayUtil.EcmaScript.Environment;
using NetDaemon.HassModel;

namespace DisplayUtil.HomeAssistant;

internal class HassJsProvider(IHaContext haContext)
    : IJsValueProvider
{
    public HassState GetState(string entityId)
    {
        var entity = haContext.GetState(entityId) ?? throw new ArgumentException($"Entity {entityId} not found");
        return new HassState(entity);
    }

    public HassState[] GetState(string[] entityIds)
    {
        return entityIds.Select(GetState).ToArray();
    }

    public void OnSetup(IJsExporter exporter)
    {
        exporter.ExposeValue("hass", this);
    }
}