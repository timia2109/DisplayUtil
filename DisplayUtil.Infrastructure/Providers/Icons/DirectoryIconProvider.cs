using DisplayUtil.Infrastructure.Utils;

namespace DisplayUtil.Infrastructure.Providers.Icons;

/// <summary>
/// An Icon Provider, which join icons by a prefix and a path
/// </summary>
/// <param name="prefix">Prefix</param>
/// <param name="path">Path</param>
public class DirectoryIconProvider(string prefix, string path) : IIconProvider
{
    private string GetFullPath(string iconName) => Path.Combine(path, $"{iconName}.svg");

    public bool CanResolve(string iconName)
    {
        var (domain, _) = iconName.SpiltDomain();
        if (!string.Equals(domain, prefix, StringComparison.OrdinalIgnoreCase))
            return false;

        return File.Exists(GetFullPath(iconName));
    }

    public Stream GetSvgIcon(string iconName)
        => File.OpenRead(GetFullPath(iconName));
}