using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// An FontAwesome Icon Element
/// </summary>
/// <param name="iconName">Name of the Icon</param>
/// <param name="height">Width of the Icon</param>
/// <param name="iconDrawer">The Icon Drawer</param>
public class IconElement : Element
{
    /// <summary>
    /// Name of the icon
    /// </summary>
    public required string IconName { get; set; }

    /// <summary>
    /// Height of the icon
    /// </summary>
    public required int Height { get; set; }

    public override void Draw(DrawContext drawContext)
    {
        drawContext.DrawResources.IconDrawer.DrawIcon(
            IconName,
            Height,
            drawContext.StartPoint,
            drawContext.Canvas
        );
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        var size = drawContext.DrawResources.IconDrawer.GetSize(IconName, Height);
        return size.GetValueOrDefault();
    }
}
