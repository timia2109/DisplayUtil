using System.Globalization;
using System.Text;
using Scriban;

namespace DisplayUtil.Template;

/// <summary>
/// Responsible to render a Scriban Template
/// </summary>
public class TemplateRenderer
{
    public async Task<Stream> RenderToStreamAsync(string content)
    {
        var context = new TemplateContext();
        context.PushCulture(CultureInfo.GetCultureInfo("de-DE"));

        var template = Scriban.Template.Parse(content);
        var rendered = await template.RenderAsync(context);

        var memoryStream = new MemoryStream();
        memoryStream.Write(Encoding.UTF8.GetBytes(rendered));
        memoryStream.Position = 0;

        return memoryStream;
    }
}