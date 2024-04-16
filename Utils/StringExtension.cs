namespace DisplayUtil.Utils;

public static class StringExtension
{
    private const char _separator = ':';

    /// <summary>
    /// Spilts a string with a domain and an item path
    /// </summary>
    /// <param name="path">Path String format: "[domain]:item"</param>
    /// <param name="defaultDomain">Fallback Domain</param>
    /// <returns>Spitted string</returns>
    /// <exception cref="Exception">There is no domain and there is no fallback</exception>
    public static (string domain, string item) SpiltDomain(
        this string path, string? defaultDomain)
    {
        string domain, icon;

        var separatorIndex = path.IndexOf(_separator);

        if (separatorIndex == -1)
        {
            if (defaultDomain is null)
            {
                throw new Exception($"No default domain defined: {path}");
            }
            domain = defaultDomain;
            icon = path;
        }
        else
        {
            domain = path[0..separatorIndex];
            icon = path[(separatorIndex + 1)..];
        }

        return (domain, icon);
    }

}