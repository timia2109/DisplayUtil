using System.Collections.Frozen;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace DisplayUtil.Providers;

/// <summary>
/// Holds the Fonts
/// </summary>
public class FontProvider(IReadOnlyDictionary<string, string> fontPathes)
{
    /// <summary>
    /// Returns the font by the internal name
    /// </summary>
    /// <param name="fontName">Name of the font</param>
    /// <returns>The Font</returns>
    /// <exception cref="Exception">Font not defined</exception>
    public SKTypeface GetFont(string fontName)
    {
        if (!fontPathes.TryGetValue(fontName, out var font))
        {
            throw new Exception($"Font {fontName} not defined!");
        }

        return SKTypeface.FromFile(font);
    }

    public static FontProvider CreateFontProvider(IServiceProvider serviceProvider)
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<ProviderSettings>>();

        var fonts = new Dictionary<string, string>(options.Value.Fonts);

        // Handle GFonts
        var gfontDownloader = ActivatorUtilities
            .CreateInstance<GFontDownloader>(serviceProvider);

        var gfonts = fonts
            .Where(kv => gfontDownloader.IsGFont(kv.Value));

        foreach (var (k, v) in gfonts)
        {
            var path = gfontDownloader.DownloadFontAsync(v).Result;
            fonts[k] = path;
        }

        // Check if fonts exists
        var missingFiles = fonts
            .Except(gfonts)
            .Where(e => !File.Exists(e.Value))
            .ToArray();

        if (missingFiles.Length != 0)
        {
            throw new Exception($"Missing fonts detected. Please check existence of: {string.Join(", ", missingFiles)}");
        }

        return new FontProvider(fonts.ToFrozenDictionary());
    }
}
