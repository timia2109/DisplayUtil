using System.Collections.Frozen;
using SkiaSharp;

namespace DisplayUtil.Utils;

/// <summary>
/// Holds the Fonts
/// </summary>
public class FontProvider(IReadOnlyDictionary<string, SKTypeface> _fonts)
{
    private const string FontPath = "./Resources/fonts";

    public SKTypeface GetFont(string fontName)
    {
        if (!_fonts.TryGetValue(fontName, out var font))
        {
            throw new Exception($"Font {fontName} not defined!");
        }

        return font;
    }

    public static FontProvider Create()
    {
        var items = Directory.GetFiles(FontPath)
            .Select(e => new { Path = e, Name = Path.GetFileNameWithoutExtension(e) })
            .ToDictionary(e => e.Name, v => SKTypeface.FromFile(v.Path))
            .ToFrozenDictionary();

        return new FontProvider(items);
    }
}
