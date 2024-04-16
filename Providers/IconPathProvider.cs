using DisplayUtil.Utils;
using Microsoft.Extensions.Options;

namespace DisplayUtil.Providers;

/// <summary>
/// Provides the path to an icon
/// </summary>
public class IconPathProvider(IOptions<ProviderSettings> options)
{
    private const char _separator = ':';

    private ProviderSettings _settings => options.Value;

    public string ResolvePath(string path)
    {
        var (domain, icon) = path.SpiltDomain(_settings.DefaultIcons);
        return ResolvePath(domain, icon);
    }

    public string ResolvePath(string domain, string iconName)
    {
        var basePath = _settings.Icons[domain];

        return Path.Combine(basePath, $"{iconName}.svg");
    }
}