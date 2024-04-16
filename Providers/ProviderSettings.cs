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
}