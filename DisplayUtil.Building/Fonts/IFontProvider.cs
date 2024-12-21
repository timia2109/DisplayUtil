using SkiaSharp;

namespace DisplayUtil.Building.Font;

/// <summary>
/// Provides a Font by its name
/// </summary>
public interface IFontProvider
{
    /// <summary>
    /// Resolves a font by it's name
    /// </summary>
    /// <param name="fontName">Name of the font</param>
    /// <returns>The Font</returns>
    /// <exception cref="FontNotFoundException">The Font was not found</exception>
    SKTypeface GetFont(string fontName);
}