using System.Globalization;
using DisplayUtil.Template;
using NetDaemon.HassModel;
using Scriban.Runtime;

namespace DisplayUtil.HomeAssistant;

internal class HassTemplateExtender(IHaContext haContext)
: ITemplateExtender
{
    public void Enrich(ScriptObject context, EnrichScope scope)
    {
        // Hass Functions
        var hassObject = new ScriptObject();
        hassObject.Import("get_state", GetState);
        hassObject.Import("get_attribute", GetAttribute);
        hassObject.Import("get_float_state", GetFloatState);
        hassObject.Import("get_datetime", GetDateTime);
        context.Add("hass", hassObject);
    }

    private string? GetState(string entityId)
    {
        var entity = haContext.GetState(entityId);
        return entity?.State;
    }

    private string? GetAttribute(string entityId, string attribute)
    {
        var entity = haContext.GetState(entityId);
        var attributes = entity?.Attributes as Dictionary<string, object?>;
        object? value = null;

        if (!attributes?.TryGetValue(attribute, out value) ?? true)
            return null;

        return value?.ToString();
    }

    private float GetFloatState(string entityId)
    {
        var state = GetState(entityId);
        if (state == null) return 0f;
        return UtilTemplateExtender.ToFloat(state);
    }

    private DateTime? GetDateTime(string entityId)
    {
        var state = GetState(entityId);
        if (state is null) return null;

        if (!DateTime.TryParseExact(state,
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.GetCultureInfo("de-DE"),
            DateTimeStyles.None,
            out var dt))
            return null;

        return dt;
    }
}