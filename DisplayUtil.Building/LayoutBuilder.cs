using DisplayUtil.Building;
using DisplayUtil.Building.Font;
using DisplayUtil.Layouting.Utils;
using SkiaSharp;

namespace DisplayUtil.Layouting;

public interface ILayoutBuilder
{
    Element Build();
}

/// <summary>
/// Helper to simplify the creation of Layouts
/// </summary>
public abstract class LayoutBuilder<TBuilder>(
    IFontProvider fontProvider,
    IIconDrawer iconDrawer,
    DefaultDefinition defaultDefinition
) : ILayoutBuilder
where TBuilder : LayoutBuilder<TBuilder>
{
    protected IFontProvider fontProvider = fontProvider;
    protected IIconDrawer iconDrawer = iconDrawer;
    protected DefaultDefinition defaultDefinition = defaultDefinition;

    public DefaultDefinition DefaultDefinition { get; protected set; }
        = defaultDefinition;

    public TBuilder SetFontSize(int size)
    {
        DefaultDefinition = DefaultDefinition with { TextSize = size };
        return (TBuilder)this;
    }

    public TBuilder SetFont(string fontName)
    {
        DefaultDefinition = DefaultDefinition with { Font = fontName };
        return (TBuilder)this;
    }

    public TBuilder SetIconHeight(int height)
    {
        DefaultDefinition = DefaultDefinition with { IconHeight = height };
        return (TBuilder)this;
    }

    protected IconElement CreateIcon(string iconName, int? height)
        => new(iconName, height ?? DefaultDefinition.IconHeight, iconDrawer);

    protected TextElement CreateText(string text, string? fontName, int? fontSize)
    {
        var font = fontProvider.GetFont(fontName ?? DefaultDefinition.Font);
        var paint = new SKPaint
        {
            IsAntialias = true,
            TextSize = fontSize ?? DefaultDefinition.TextSize,
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            Typeface = font
        };
        return new TextElement(text, paint);
    }

    public abstract Element Build();
}