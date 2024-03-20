using System.Globalization;
using System.Text;
using Scriban;

namespace DisplayUtil.Template;

/// <summary>
/// Responsible to render a Scriban Template.
/// Scoped
/// </summary>
public class TemplateRenderer(TemplateContextProvider contextProvider)
{
    public async Task<Stream> RenderToStreamAsync(string content)
    {
        var template = Scriban.Template.Parse(content);
        var rendered = await template.RenderAsync(contextProvider
            .GetTemplateContext());

        var memoryStream = new MemoryStream();
        memoryStream.Write(Encoding.UTF8.GetBytes(rendered));
        memoryStream.Position = 0;

        return memoryStream;
    }
}