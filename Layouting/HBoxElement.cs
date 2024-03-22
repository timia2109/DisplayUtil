using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// A HBox (Draws content next to each other)
/// </summary>
/// <param name="gap">Gap between Items</param>
public class HBoxElement(int gap = 0) : ElementCollection
{
    protected override void DrawCollection(DrawContext drawContext)
    {
        var point = drawContext.StartPoint;
        foreach (var child in Children)
        {
            var size = child.GetSize(drawContext);
            child.Draw(new DrawContext(
                drawContext.Canvas,
                drawContext.Size,
                point
            ));

            point.X += size.Width + gap;
        }
    }

    protected override SKSize CalculateCollectionSize(DrawContext drawContext)
    {
        var childrenSize = GetChildrenSizes(drawContext);
        return new SKSize(
            childrenSize.WidthSum + (gap * (Children.Count - 1)),
            childrenSize.MaxHeight
        );
    }
}
