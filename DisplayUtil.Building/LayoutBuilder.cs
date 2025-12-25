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
///     Helper to simplify the creation of Layouts
/// </summary>
public abstract class LayoutBuilder<TBuilder>(
    IFontProvider fontProvider,
    IIconDrawer iconDrawer,
    DefaultDefinition defaultDefinition
) : DefaultDefinitionBuilder<TBuilder>(defaultDefinition), ILayoutBuilder
    where TBuilder : LayoutBuilder<TBuilder>
{
    protected SiteSizeBuilder? _border;
    protected SiteSizeBuilder? _margin;
    protected SiteSizeBuilder? _padding;
    protected IFontProvider fontProvider = fontProvider;
    protected IIconDrawer iconDrawer = iconDrawer;

    public abstract Element Build();

    protected IconElement CreateIcon(string iconName, int? height)
    {
        return new IconElement(iconName, height ?? DefaultDefinition.IconHeight, iconDrawer);
    }

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

    public TBuilder WithPadding(Action<SiteSizeBuilder> configure)
    {
        _padding = new SiteSizeBuilder();
        configure(_padding);
        return (TBuilder)this;
    }

    public TBuilder WithMargin(Action<SiteSizeBuilder> configure)
    {
        _margin = new SiteSizeBuilder();
        configure(_margin);
        return (TBuilder)this;
    }

    public TBuilder WithBorder(Action<SiteSizeBuilder> configure)
    {
        _border = new SiteSizeBuilder();
        configure(_border);
        return (TBuilder)this;
    }
}