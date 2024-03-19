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
public class FlexboxElement : ElementCollection
{
    private readonly int _gap;
    private readonly FlexDirection _direction;
    private readonly JustifyContent _justifyContent;
    private readonly AlignItems _alignItems;

    public FlexboxElement(int gap = 0, FlexDirection direction = FlexDirection.Horizontal, JustifyContent justifyContent = JustifyContent.Start, AlignItems alignItems = AlignItems.Start)
    {
        _gap = gap;
        _direction = direction;
        _justifyContent = justifyContent;
        _alignItems = alignItems;
    }

    public override void Draw(DrawContext drawContext)
    {
        var point = drawContext.StartPoint;
        var sizes = Children.Select(child => child.GetSize(drawContext)).ToList();
        var totalSize = _direction == FlexDirection.Horizontal ? sizes.Sum(size => size.Width) : sizes.Sum(size => size.Height);
        var totalGapSize = _gap * (Children.Count - 1);
        var containerSize = _direction == FlexDirection.Horizontal ? drawContext.Size.Width : drawContext.Size.Height;
        var crossAxisSize = _direction == FlexDirection.Horizontal ? drawContext.Size.Height : drawContext.Size.Width;
        var remainingSpace = containerSize - totalSize;
        var spaceBetween = _justifyContent == JustifyContent.Between && Children.Count > 1 ? remainingSpace / (Children.Count - 1) + _gap : _gap;

        for (int i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            var childSize = sizes[i];
            var alignmentOffset = CalculateAlignmentOffset(crossAxisSize, childSize, _direction, _alignItems);

            var alignedPoint = new SKPoint(
                _direction == FlexDirection.Horizontal ? point.X : point.X + alignmentOffset,
                _direction == FlexDirection.Vertical ? point.Y : point.Y + alignmentOffset
            );

            child.Draw(new DrawContext(drawContext.Canvas, drawContext.Size, alignedPoint));

            if (_direction == FlexDirection.Horizontal)
            {
                point.X += childSize.Width + spaceBetween;
            }
            else // FlexDirection.Vertical
            {
                point.Y += childSize.Height + spaceBetween;
            }
        }
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        var sizes = Children.Select(child => child.GetSize(drawContext)).ToList();
        var totalWidth = _direction == FlexDirection.Horizontal ? sizes.Sum(size => size.Width) + (_gap * (Children.Count - 1)) : sizes.Max(size => size.Width);
        var totalHeight = _direction == FlexDirection.Vertical ? sizes.Sum(size => size.Height) + (_gap * (Children.Count - 1)) : sizes.Max(size => size.Height);

        return new SKSize(totalWidth, totalHeight);
    }

    private float CalculateAlignmentOffset(float crossAxisSize, SKSize childSize, FlexDirection direction, AlignItems alignItems)
    {
        switch (alignItems)
        {
            case AlignItems.Start:
                return 0;
            case AlignItems.End:
                return direction == FlexDirection.Horizontal ? crossAxisSize - childSize.Height : crossAxisSize - childSize.Width;
            case AlignItems.Center:
                return (crossAxisSize - (direction == FlexDirection.Horizontal ? childSize.Height : childSize.Width)) / 2;
            default:
                return 0;
        }
    }
}
