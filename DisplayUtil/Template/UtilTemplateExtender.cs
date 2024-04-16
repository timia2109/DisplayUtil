using System.Globalization;
using Scriban.Runtime;

namespace DisplayUtil.Template;

internal class UtilTemplateExtender : ITemplateExtender
{
    public void Enrich(ScriptObject scriptObject, EnrichScope scope)
    {
        scriptObject.Import("to_float", ToFloat);

        var timespanObject = new ScriptObject();
        timespanObject.Import(typeof(TimeSpan));
        timespanObject.Import("to_string", TimespanToString);
        scriptObject.Add("timespan", timespanObject);
    }

    public static float ToFloat(string? content)
    {
        if (content == null) return 0;
        return float.Parse(content, CultureInfo.InvariantCulture);
    }

    public static string TimespanToString(TimeSpan timeSpan, string format)
    {
        return timeSpan.ToString(format);
    }
}