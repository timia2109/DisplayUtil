using System.Globalization;
using System.Text;
using DisplayUtil.MqttExport;
using Microsoft.Extensions.Options;
using Scriban;
using Scriban.Parsing;

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
    private async Task<Scriban.Template> GetTemplateAsync(
        string templateName,
        LexerOptions lexerOptions
    )
    {
        var path = Path.IsPathFullyQualified(templateName) ?
            templateName
            : loader.GetPath(templateName);

        var content = await loader.LoadAsync(path);
        return Scriban.Template.Parse(content, templateName, null,
            lexerOptions);
    }

    /// <summary>
    /// Evaluates a Scriban Script
    /// </summary>
    /// <param name="templateName">Name of the Template</param>
    /// <returns>Task</returns>
    public async Task EvaluateAsync(string templateName)
    {
        var template = await GetTemplateAsync(
            templateName,
            new LexerOptions() { Mode = ScriptMode.ScriptOnly }
        );
        var context = contextProvider.GetTemplateContext(EnrichScope.Job);

        await template.EvaluateAsync(context);
    }

    /// <summary>
    /// Renders a Scriban Template
    /// </summary>
    /// <param name="templateName">Name of the Template</param>
    /// <param name="scope">EnrichScope</param>
    /// <returns>Result of the rendering</returns>
    public async Task<string> RenderAsync(
        string templateName, EnrichScope scope
    )
    {
        var template = await GetTemplateAsync(templateName,
            LexerOptions.Default);
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