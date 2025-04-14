using System.Globalization;
using NetDaemon.HassModel;

namespace DisplayUtil.Home.HomeAssistant;

public class HassUtil(IHaContext haContext)
{
    public string? GetState(string entityId)
    {
        var entity = haContext.GetState(entityId);
        return entity?.State;
    }

    public string? GetAttribute(string entityId, string attribute)
    {
        var entity = haContext.GetState(entityId);
        var attributes = entity?.Attributes as Dictionary<string, object?>;
        object? value = null;

        if (!attributes?.TryGetValue(attribute, out value) ?? true)
            return null;

        return value?.ToString();
    }

    public double GetFloatState(string entityId)
    {
        var state = GetState(entityId);
        if (state == null) return 0f;
        return Convert.ToDouble(state);
    }

    public DateTime? GetDateTime(string entityId)
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