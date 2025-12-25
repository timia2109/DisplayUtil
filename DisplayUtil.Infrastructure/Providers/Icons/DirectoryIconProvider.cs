using DisplayUtil.Infrastructure.Utils;

namespace DisplayUtil.Infrastructure.Providers.Icons;

/// <summary>
///     An Icon Provider, which join icons by a prefix and a path
/// </summary>
/// <param name="prefix">Prefix</param>
/// <param name="path">Path</param>
public class DirectoryIconProvider(string prefix, string path) : IIconProvider
{
    public bool CanResolve(string iconName)
    {
        var (domain, iconElement) = iconName.SpiltDomain();
        if (!string.Equals(domain, prefix, StringComparison.OrdinalIgnoreCase))
            return false;

        return File.Exists(GetFullPath(iconElement));
    }

    public Stream GetSvgIcon(string iconName)
    {
        return File.OpenRead(GetFullPath(iconName.SpiltDomain().item));
    }

    private string GetFullPath(string iconName)
    {
        return Path.Combine(path, $"{iconName}.svg");
    }
}