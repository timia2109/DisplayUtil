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
public class TemplateContextProvider(IHaContext haContext)
{
    public TemplateContext GetTemplateContext()
    {
        var scriptObject = new ScriptObject();
        scriptObject.Import("hass_get_state", GetState);
        scriptObject.Import("hass_get_attribute", GetAttribute);

        var context = new TemplateContext();
        context.PushCulture(CultureInfo.GetCultureInfo("de-DE"));
        context.PushGlobal(scriptObject);
        //context.MemberFilter = MemberFilter;

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

    private bool MemberFilter(MemberInfo member)
    {
        return member switch
        {
            MethodInfo m => m.IsPublic,
            PropertyInfo p => p.IsPubliclyReadable(),
            _ => false
        };
    }

}
