using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
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
        scriptObject.Import("get_state", haContext.GetState);

        var context = new TemplateContext();
        context.PushCulture(CultureInfo.GetCultureInfo("de-DE"));
        context.PushGlobal(scriptObject);
        //context.MemberFilter = MemberFilter;

        return context;
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
