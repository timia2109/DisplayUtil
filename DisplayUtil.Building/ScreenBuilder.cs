using DisplayUtil.Building.Font;
using DisplayUtil.Building.Items;
using DisplayUtil.Layouting;
using DisplayUtil.Layouting.Utils;
using SkiaSharp;

namespace DisplayUtil.Building;

public class ScreenBuilder(
    IFontProvider fontProvider,
    IIconDrawer iconDrawer,
    DefaultDefinition defaultDefinition)
    : LayoutBuilder<ScreenBuilder>(fontProvider, iconDrawer, defaultDefinition)
{
    private ILayoutBuilder? _rootBuilder;

    public int Height { get; private set; }
    public int Width { get; private set; }

    public ScreenBuilder WithWidth(int width)
    {
        Width = width;
        return this;
    }

    public ScreenBuilder WithHeight(int height)
    {
        Height = height;
        return this;
    }

    private TBuilder Use<TBuilder>(TBuilder builder)
        where TBuilder : ILayoutBuilder
    {
        _rootBuilder = builder;
        return builder;
    }

    public FlexboxBuilder UseFlexbox()
    {
        return Use(new FlexboxBuilder(fontProvider, iconDrawer, DefaultDefinition));
    }

    public VBoxBuilder UseVBox()
    {
        return Use(new VBoxBuilder(fontProvider, iconDrawer, DefaultDefinition));
    }

    public HBoxBuilder UseHBox()
    {
        return Use(new HBoxBuilder(fontProvider, iconDrawer, DefaultDefinition));
    }

    public override Element Build()
    {
        if (_rootBuilder == null) throw new InvalidOperationException("Root element is not set");

        return _rootBuilder.Build();
    }

    /// <summary>
    ///     Builds the screen and returns the bitmap
    /// </summary>
    /// <returns>Bitmap of screen</returns>
    public SKBitmap Draw()
    {
        var element = Build();
        var size = new SKSize(Width, Height);
        return DrawManager.Draw(size, element);
    }
}