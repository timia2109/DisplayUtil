using DisplayUtil.Layouting;
using SkiaSharp;

namespace DisplayUtil.Layouting;

public enum FlexDirection
{
    Horizontal,
    Vertical
}

public enum JustifyContent
{
    Start,
    Between
}

public enum AlignItems
{
    Start,
    End,
    Center
}

/// <summary>
/// A FlexboxElement that supports direction, justify content between, utilizes full width or height, and aligns items.
/// </summary>
public class FlexboxElement : GapElementCollection
{
    public FlexDirection Direction { get; set; } = FlexDirection.Horizontal;
    public JustifyContent JustifyContent { get; set; } = JustifyContent.Start;
    public AlignItems AlignItems { get; set; } = AlignItems.Start;


    protected override void DrawCollection(DrawContext drawContext)
    {
        var point = drawContext.StartPoint;
        var sizes = Children.Select(child => child.GetSize(drawContext)).ToList();
        var totalSize = Direction == FlexDirection.Horizontal ? sizes.Sum(size => size.Width) : sizes.Sum(size => size.Height);
        var totalGapSize = Gap * (Children.Count - 1);
        var containerSize = Direction == FlexDirection.Horizontal ? drawContext.Size.Width : drawContext.Size.Height;
        //var crossAxisSize = _direction == FlexDirection.Horizontal ? drawContext.Size.Height : drawContext.Size.Width;
        var crossAxisSize = Direction == FlexDirection.Horizontal ? sizes.Max(e => e.Height) : sizes.Max(e => e.Width);
        var remainingSpace = containerSize - totalSize;
        var spaceBetween = JustifyContent == JustifyContent.Between && Children.Count > 1 ? remainingSpace / (Children.Count - 1) + Gap : Gap;

        for (int i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            var childSize = sizes[i];
            var alignmentOffset = CalculateAlignmentOffset(crossAxisSize, childSize, Direction, AlignItems);

            var alignedPoint = new SKPoint(
                Direction == FlexDirection.Horizontal ? point.X : point.X + alignmentOffset,
                Direction == FlexDirection.Vertical ? point.Y : point.Y + alignmentOffset
            );

            child.Draw(drawContext with { StartPoint = alignedPoint });

            if (Direction == FlexDirection.Horizontal)
            {
                point.X += childSize.Width + spaceBetween;
            }
            else // FlexDirection.Vertical
            {
                point.Y += childSize.Height + spaceBetween;
            }
        }
    }

    protected override SKSize CalculateCollectionSize(DrawContext drawContext)
    {
        var containerSize = Direction == FlexDirection.Horizontal ? drawContext.Size.Width : drawContext.Size.Height;
        var lineLength = 0f;
        var totalCrossSize = 0f;
        var maxLineCrossSize = 0f;

        foreach (var child in Children.Select(child => child.GetSize(drawContext)))
        {
            var childMainSize = Direction == FlexDirection.Horizontal ? child.Width : child.Height;
            var childCrossSize = Direction == FlexDirection.Horizontal ? child.Height : child.Width;

            // If adding this child would exceed the container size, move to a new line
            if (lineLength + childMainSize > containerSize && lineLength > 0)
            {
                totalCrossSize += maxLineCrossSize + Gap; // Add the largest item on the previous line and gap
                lineLength = 0; // Reset line length for new line
                maxLineCrossSize = 0; // Reset max height for new line
            }

            lineLength += childMainSize + (Children.Count > 1 ? Gap : 0); // Add child length and gap if not the first item
            maxLineCrossSize = Math.Max(maxLineCrossSize, childCrossSize); // Update max height if this item is taller
        }

        totalCrossSize += maxLineCrossSize; // Add the last line's max height

        // The total size is the larger of the total length of all lines and the container size, and the total cross size
        return Direction == FlexDirection.Horizontal ? new SKSize(Math.Max(lineLength - Gap, containerSize), totalCrossSize) : new SKSize(totalCrossSize, Math.Max(lineLength - Gap, containerSize));
    }

    private float CalculateAlignmentOffset(float crossAxisSize, SKSize childSize, FlexDirection direction, AlignItems alignItems)
    {
        return alignItems switch
        {
            AlignItems.Start => 0,
            AlignItems.End => direction == FlexDirection.Horizontal ? crossAxisSize - childSize.Height : crossAxisSize - childSize.Width,
            AlignItems.Center => (crossAxisSize - (direction == FlexDirection.Horizontal ? childSize.Height : childSize.Width)) / 2,
            _ => 0,
        };
    }
}
