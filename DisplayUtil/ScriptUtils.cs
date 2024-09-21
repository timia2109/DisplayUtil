using System.Globalization;

namespace DisplayUtil;

public static class ScriptUtils
{
    public static float ToFloat(string? content)
    {
        if (content == null) return 0;
        if (float.TryParse(content, CultureInfo.InvariantCulture, out var value))
        {
            return value;
        }
        return 0;
    }

    public static string TimespanToString(TimeSpan timeSpan, string format)
    {
        return timeSpan.ToString(format);
    }
}