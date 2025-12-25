namespace DisplayUtil.Infrastructure.Providers.Icons;

/// <summary>
///     Configuration for Icons
/// </summary>
public record IconConfiguration
{
    /// <summary>
    ///     Key is the prefix of the icon, value is the path to the icon directory.
    /// </summary>
    public Dictionary<string, string> Icons { get; init; } = null!;
}