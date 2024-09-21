using System.Globalization;
using NetDaemon.HassModel.Entities;

namespace DisplayUtil.HomeAssistant;

/// <summary>
/// Abstract Wrapper for an Home Assistant Value.
/// Provides a way to get the value as a string, float, binary or DateTime.
/// </summary>
public abstract record HassStateValue
{
    protected abstract string? Value { get; }

    public string? GetString()
    {
        return Value;
    }

    public bool GetBoolean()
    {
        return Value == "on";
    }

    public float GetFloat()
    {
        var state = Value;
        if (state == null) return 0f;
        return ScriptUtils.ToFloat(state);
    }

    public DateTime? GetDateTime()
    {
        var state = Value;
        if (state is null) return null;

        if (DateTime.TryParse(state, out var isodt))
            return isodt;
        if (DateTime.TryParseExact(state,
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.GetCultureInfo("de-DE"),
            DateTimeStyles.None,
            out var dt))
            return dt;

        return null;
    }
}

public record HassState(EntityState? state) : HassStateValue
{
    public EntityState? LatestState { get; } = state;
    protected override string? Value => LatestState?.State;

    public HassAttributeValue? GetAttribute(string attribute)
    {
        if (LatestState?.Attributes is not Dictionary<string, object> attributes) return null;

        object? value = null;

        if (!attributes?.TryGetValue(attribute, out value) ?? true)
            return null;

        return value == null ? null : new HassAttributeValue(value.ToString()!);
    }
}

public record HassAttributeValue(string AttributeValue) : HassStateValue
{
    protected override string? Value => AttributeValue;
}