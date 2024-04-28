using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// An FontAwesome Icon Element
/// </summary>
/// <param name="iconName">Name of the Icon</param>
/// <param name="height">Width of the Icon</param>
/// <param name="iconDrawer">The Icon Drawer</param>
public class IconElement(string iconName, int height, IconDrawer iconDrawer) : Element
{
    public override void Draw(DrawContext drawContext)
    {
        iconDrawer.DrawIcon(
            iconName,
            height,
            drawContext.StartPoint,
            drawContext.Canvas
        );
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        var size = iconDrawer.GetSize(iconName, height);
        return size.GetValueOrDefault();
    }
}
