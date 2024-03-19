using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// An FontAwesome Icon Element
/// </summary>
/// <param name="iconName">Name of the Icon</param>
/// <param name="width">Width of the Icon</param>
/// <param name="iconDrawer">The Icon Drawer</param>
public class IconElement(string iconName, int width, FaIconDrawer iconDrawer) : Element
{
    public override void Draw(DrawContext drawContext)
    {
        iconDrawer.DrawIcon(
            iconName,
            width,
            drawContext.StartPoint,
            drawContext.Canvas
        );
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        var size = iconDrawer.GetSize(iconName, width);
        return size.GetValueOrDefault();
    }
}
