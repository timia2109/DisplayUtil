namespace DisplayUtil.Providers;

public record ProviderSettings
{
    /// <summary>
    /// Fonts.
    /// Key = Internal name
    /// Value = Font Path || gfonts://font-name (downloaded once)
    /// </summary>
    public IReadOnlyDictionary<string, string> Fonts { get; init; }
        = new Dictionary<string, string>();

    /// <summary>
    /// Path where Google Fonts gets cached to
    /// </summary>
    public string GoogleFontsCachePath { get; init; } = null!;

    /// <summary>
    /// Specifies the available icons
    /// Key = Protocol
    /// Value = Path to SVG Folder
    /// </summary>
    public IReadOnlyDictionary<string, string> Icons { get; init; }
        = new Dictionary<string, string>();

    /// <summary>
    /// Defines the default Icons
    /// </summary>
    public string? DefaultIcons { get; init; }
}