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
        string domain,
            icon;

        var separatorIndex = path.IndexOf(_separator);

        if (separatorIndex == -1)
        {
            if (_settings.DefaultIcons is null)
            {
                throw new Exception($"No icon domain defined: {path}");
            }
            domain = _settings.DefaultIcons;
            icon = path;
        }
        else
        {
            domain = path[0..separatorIndex];
            icon = path[(separatorIndex + 1)..];
        }

        return ResolvePath(domain, icon);
    }

    public string ResolvePath(string domain, string iconName)
    {
        var basePath = _settings.Icons[domain];

        return Path.Combine(basePath, $"{iconName}.svg");
    }
}