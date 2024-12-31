namespace DisplayUtil.Infrastructure.Providers.Font;

/// <summary>
/// Static bean to configuring fonts from the configuration.
/// </summary>
public record FontConfiguration
{
    /// <summary>
    /// Key is the font name, value is the path to the font file.
    /// </summary>
    public Dictionary<string, string> LocalFonts { get; init; } = null!;

    /// <summary>
    /// Key is the font name, value is the font name in Google Fonts.
    /// </summary>
    public Dictionary<string, string> GoogleFonts { get; init; } = null!;

    /// <summary>
    /// The directory to store the Google Fonts.
    /// </summary>
    public string GoogleFontDirectory { get; init; } = null!;

    /// <summary>
    /// Apply the configuration to the Font Provider Builder.
    /// </summary>
    /// <param name="fontProviderBuilder">Font Provider Builder</param>
    public void Apply(FontProviderBuilder fontProviderBuilder)
    {
        foreach (var font in LocalFonts)
        {
            fontProviderBuilder.AddLocalFont(font.Key, font.Value);
        }

        foreach (var font in GoogleFonts)
        {
            fontProviderBuilder.AddGoogleFont(font.Key, font.Value);
        }
        fontProviderBuilder.SetGoogleFontDirectory(GoogleFontDirectory);
    }
}