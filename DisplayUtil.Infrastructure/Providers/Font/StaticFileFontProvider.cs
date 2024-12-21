using DisplayUtil.Building.Font;
using SkiaSharp;

namespace DisplayUtil.Infrastructure.Providers.Font;

public class StaticFileFontProvider(
    IReadOnlyDictionary<string, string> fontPaths) : IFontProvider
{
    public SKTypeface GetFont(string fontName)
    {
        if (!fontPaths.TryGetValue(fontName, out var path))
        {
            throw new FontNotFoundException(fontName);
        }

        return SKTypeface.FromFile(path);
    }
}