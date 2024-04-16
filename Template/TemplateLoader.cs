using Microsoft.Extensions.Options;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace DisplayUtil.Template;

public class TemplateLoader : ITemplateLoader
{
    private readonly string[] _searchPathes;
    private static readonly string[] _allowedExtensions = [
        "sbntxt", "sbnxml"
    ];

    public TemplateLoader(IOptions<TemplateSettings> options)
    {
        var settings = options.Value;

        _searchPathes = settings.Paths
            .OrderBy(k => k.Key)
            .Select(v => Path.GetFullPath(v.Value))
            .ToArray();
    }

    public string GetPath(string templateName)
    {
        var fileNames = _allowedExtensions
            .Select(e => $"{templateName}.{e}");

        var paths = fileNames
            .SelectMany(f =>
                _searchPathes.Select(p => Path.Combine(p, f))
            );

        var path = paths
            .First(File.Exists);

        return path;
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