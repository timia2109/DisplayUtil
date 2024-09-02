using DisplayUtil.Utils;
using Microsoft.Extensions.Options;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace DisplayUtil.Template;

public class TemplateLoader(IOptions<TemplateSettings> options) : ITemplateLoader
{
    private static readonly string[] _allowedExtensions = [
        "sbntxt", "sbnxml", "sbn"
    ];

    public string? GetPath(string templateName)
    {
        var (domain, item) = templateName.SpiltDomain(null);

        if (!options.Value.Paths.TryGetValue(domain, out var directory))
            return null;

        var path = _allowedExtensions
            .Select(e => $"{item}.{e}")
            .Select(f => Path.Combine(directory, f))
            .First(File.Exists);

        return Path.GetFullPath(path);
    }

    public string? GetPath(
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