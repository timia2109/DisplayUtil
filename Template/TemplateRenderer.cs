using System.Globalization;
using System.Text;
using DisplayUtil.MqttExport;
using Microsoft.Extensions.Options;
using Scriban;

namespace DisplayUtil.Template;

/// <summary>
/// Responsible to render a Scriban Template.
/// Scoped
/// </summary>
public class TemplateRenderer(
    TemplateContextProvider contextProvider,
    TemplateLoader loader
)
{
    public async Task<string> RenderAsync(
        string templateName, EnrichScope scope
    )
    {
        var path = Path.IsPathFullyQualified(templateName) ?
            templateName
            : loader.GetPath(templateName);

        var content = await loader.LoadAsync(path);
        var template = Scriban.Template.Parse(content);
        var context = contextProvider.GetTemplateContext(scope);

        return await template.RenderAsync(context);
    }

    public async Task<Stream> RenderToStreamAsync(string templateName)
    {
        var rendered = await RenderAsync(templateName,
            EnrichScope.ScreenRendering);

        var memoryStream = new MemoryStream();
        memoryStream.Write(Encoding.UTF8.GetBytes(rendered));
        memoryStream.Position = 0;

        return memoryStream;
    }
}