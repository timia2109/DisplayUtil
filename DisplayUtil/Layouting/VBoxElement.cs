using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// A VBox (Draws content under each other)
/// </summary>
/// <param name="gap">Gap between Items</param>
public class VBoxElement(int gap = 0) : ElementCollection
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

            point.Y += size.Height + gap;
        }
    }

    protected override SKSize CalculateCollectionSize(DrawContext drawContext)
    {
        var childrenSize = GetChildrenSizes(drawContext);
        return new SKSize(
            childrenSize.MaxWidth,
            childrenSize.HeightSum + (gap * (Children.Count - 1))
        );
    }
}
