using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace DisplayUtil.Template;

public class TemplateLoader : ITemplateLoader
{
    private const string TemplatePath = "./Resources/screens";

    public string GetPath(string templateName)
    {
        return Path.GetFullPath(
            Path.Combine(TemplatePath, $"{templateName}.sbntxt")
        );
    }

    public string GetPath(
        TemplateContext context,
        SourceSpan callerSpan,
        string templateName
    )
    {
        return GetPath(templateName);
    }

    public string Load(TemplateContext context,
        SourceSpan callerSpan, string templatePath)
    {
        return File.ReadAllText(templatePath);
    }

    public async ValueTask<string> LoadAsync(TemplateContext context,
        SourceSpan callerSpan, string templatePath)
    {
        return await LoadAsync(templatePath);
    }

    public Task<string> LoadAsync(string templatePath)
    {
        return File.ReadAllTextAsync(templatePath);
    }
}