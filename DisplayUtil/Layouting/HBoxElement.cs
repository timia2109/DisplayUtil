using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// A HBox (Draws content next to each other)
/// </summary>
/// <param name="gap">Gap between Items</param>
public class HBoxElement : GapElementCollection
{
    protected override void DrawCollection(DrawContext drawContext)
    {
        var point = drawContext.StartPoint;
        foreach (var child in Children)
        {
            var size = child.GetSize(drawContext);
            child.Draw(drawContext with { StartPoint = point });

            point.X += size.Width + Gap;
        }
    }

    protected override SKSize CalculateCollectionSize(DrawContext drawContext)
    {
        var childrenSize = GetChildrenSizes(drawContext);
        return new SKSize(
            childrenSize.WidthSum + (Gap * (Children.Count - 1)),
            childrenSize.MaxHeight
        );
    }
}
