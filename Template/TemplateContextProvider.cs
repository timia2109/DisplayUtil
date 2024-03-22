using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using NetDaemon.HassModel;
using Scriban;
using Scriban.Runtime;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DisplayUtil.Template;

/// <summary>
/// Provides the default template objects.
/// Scoped
/// </summary>
public class TemplateContextProvider(IHaContext haContext, TemplateLoader templateLoader)
{
    public TemplateContext GetTemplateContext()
    {
        var scriptObject = new ScriptObject();
        scriptObject.Import("to_float", ToFloat);

        // Hass Functions
        var hassObject = new ScriptObject();
        hassObject.Import("get_state", GetState);
        hassObject.Import("get_attribute", GetAttribute);
        hassObject.Import("get_float_state", GetFloatState);
        scriptObject.Add("hass", hassObject);

        var context = new TemplateContext
        {
            TemplateLoader = templateLoader
        };

        context.PushCulture(CultureInfo.GetCultureInfo("de-DE"));
        context.PushGlobal(scriptObject);

        return context;
    }

    private string? GetState(string entityId)
    {
        var entity = haContext.GetState(entityId);
        return entity?.State;
    }

    private string? GetAttribute(string entityId, string attribute)
    {
        var entity = haContext.GetState(entityId);
        var attributes = entity.Attributes as Dictionary<string, object?>;
        object? value = null;

        if (!attributes?.TryGetValue(attribute, out value) ?? true)
            return null;

        return value?.ToString();
    }

    private float GetFloatState(string entityId)
    {
        var state = GetState(entityId);
        if (state == null) return 0f;
        return ToFloat(state);
    }

    private float ToFloat(string? content)
    {
        if (content == null) return 0;
        return float.Parse(content, CultureInfo.InvariantCulture);
    }

}
