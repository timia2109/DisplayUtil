using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// A VBox (Draws content under each other)
/// </summary>
public class VBoxElement : GapElementCollection
{
    protected override void DrawCollection(DrawContext drawContext)
    {
        var point = drawContext.StartPoint;
        foreach (var child in Children)
        {
            var size = child.GetSize(drawContext);
            child.Draw(drawContext with
            {
                StartPoint = point
            });

            point.Y += size.Height + Gap;
        }
    }

    protected override SKSize CalculateCollectionSize(DrawContext drawContext)
    {
        var childrenSize = GetChildrenSizes(drawContext);
        return new SKSize(
            childrenSize.MaxWidth,
            childrenSize.HeightSum + (Gap * (Children.Count - 1))
        );
    }
}
