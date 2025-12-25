namespace DisplayUtil.Building.Font;

/// <summary>
///     Repesents an error where a font is not found
/// </summary>
public class FontNotFoundException(string fontName) : Exception($"Font {fontName} not found")
{
    public string FontName { get; } = fontName;
}