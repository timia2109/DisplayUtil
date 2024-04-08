using System.Globalization;
using Scriban;
using Scriban.Runtime;

namespace DisplayUtil.Template;

/// <summary>
/// Provides the default template objects.
/// Scoped
/// </summary>
public class TemplateContextProvider(
    IEnumerable<ITemplateExtender> extenders,
    TemplateLoader templateLoader)
{
    public TemplateContext GetTemplateContext(EnrichScope scope)
    {
        var scriptObject = new ScriptObject();

        foreach (var extender in extenders)
        {
            extender.Enrich(scriptObject, scope);
        }

        var context = new TemplateContext
        {
            TemplateLoader = templateLoader
        };

        context.PushCulture(CultureInfo.GetCultureInfo("de-DE"));
        context.PushGlobal(scriptObject);

        return context;
    }
}
