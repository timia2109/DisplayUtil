using System.ComponentModel;
using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// Baseclass for an ElementCollection. 
/// Adds support for Borders, Margins and Paddings
/// </summary>
public abstract class ElementCollection : Element
{
    /// <summary>
    /// Children of this collection
    /// </summary>
    public List<Element> Children { get; } = [];

    /// <summary>
    /// Sizes of Borders
    /// </summary>
    public SiteSize Border { get; set; } = new();

    /// <summary>
    /// Sizes of Margin
    /// </summary>
    public SiteSize Margin { get; set; } = new();

    /// <summary>
    /// Sizes of Padding
    /// </summary>
    public SiteSize Padding { get; set; } = new();

    private readonly Lazy<ContainerSizes> _containerSizes;

    protected ElementCollection()
    {
        _containerSizes = new(CalculateContainerSizes);
    }

    /// <summary>
    /// Adds a new Element to this Collection
    /// </summary>
    /// <param name="element">The element</param>
    public ElementCollection Append(Element element)
    {
        Children.Add(element);
        return this;
    }

    /// <summary>
    /// Add all Elements to this Collection
    /// </summary>
    /// <param name="elements">The elements</param>
    public ElementCollection Append(IEnumerable<Element> elements)
    {
        Children.AddRange(elements);
        return this;
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        var childContext = BuildChildDrawContext(drawContext);
        var size = CalculateCollectionSize(childContext);
        size.Width += _containerSizes.Value.X;
        size.Height += _containerSizes.Value.Y;

        return size;
    }

    private DrawContext BuildChildDrawContext(DrawContext drawContext)
    {
        var childSize = drawContext.Size;
        childSize.Width -= _containerSizes.Value.X;
        childSize.Height -= _containerSizes.Value.Y;

        var childStartPoint = drawContext.StartPoint;
        childStartPoint.X += _containerSizes.Value.StartX;
        childStartPoint.Y += _containerSizes.Value.StartY;

        return drawContext with
        {
            Size = childSize,
            StartPoint = childStartPoint
        };
    }

    private ContainerSizes CalculateContainerSizes()
    {
        SiteSize[] siteSizes = [Border, Margin, Padding];
        var startX = siteSizes.Aggregate(0, (s, e) => s + e.Left);
        var endX = siteSizes.Aggregate(0, (s, e) => s + e.Right);
        var startY = siteSizes.Aggregate(0, (s, e) => s + e.Top);
        var endY = siteSizes.Aggregate(0, (s, e) => s + e.Bottom);
        return new ContainerSizes(startX, startY, endX, endY);
    }

    public override void Draw(DrawContext drawContext)
    {
        var childDrawContext = BuildChildDrawContext(drawContext);
        DrawCollection(childDrawContext);
        DrawBorder(drawContext);
    }

    protected abstract void DrawCollection(DrawContext context);
    protected abstract SKSize CalculateCollectionSize(DrawContext drawContext);

    /// <summary>
    /// Calculates the sizes of the children
    /// </summary>
    /// <param name="drawContext">Draw Context</param>
    /// <returns>Children Size Information</returns>
    protected ChildrenSizes GetChildrenSizes(DrawContext drawContext)
    {
        return Children.Aggregate(new ChildrenSizes(),
            (sum, e) => sum + e.GetSize(drawContext)
        );
    }

    protected record struct ChildrenSizes
    {
        public float WidthSum { get; init; }
        public float HeightSum { get; init; }
        public float MaxHeight { get; init; }
        public float MaxWidth { get; init; }

        public static ChildrenSizes operator +(ChildrenSizes sizes, SKSize elementSize)
        {
            return new ChildrenSizes
            {
                HeightSum = sizes.HeightSum + elementSize.Height,
                WidthSum = sizes.WidthSum + elementSize.Width,
                MaxHeight = Math.Max(sizes.MaxHeight, elementSize.Height),
                MaxWidth = Math.Max(sizes.MaxWidth, elementSize.Width)
            };
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        foreach (var child in Children)
        {
            child.Dispose();
        }
    }

    private void DrawBorder(DrawContext drawContext)
    {
        var size = GetSize(drawContext);

        var topStart = new SKPoint(
            drawContext.StartPoint.X + Margin.Left,
            drawContext.StartPoint.Y + Margin.Top
        );

        var bottomEnd = new SKPoint(
            drawContext.StartPoint.X + size.Width - Margin.Right,
            drawContext.StartPoint.Y + size.Height - Margin.Bottom
        );

        var topEnd = new SKPoint(
            bottomEnd.X,
            topStart.Y
        );

        var bottomStart = new SKPoint(
            topStart.X,
            bottomEnd.Y
        );

        DrawLine(drawContext, topStart, topEnd, Border.Top);
        DrawLine(drawContext, topStart, bottomStart, Border.Left);
        DrawLine(drawContext, bottomStart, bottomEnd, Border.Bottom);
        DrawLine(drawContext, topEnd, bottomEnd, Border.Right);
    }

    private static void DrawLine(DrawContext drawContext, SKPoint startPoint,
        SKPoint endPoint, int width)
    {
        if (width == 0) return;

        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = width,
            Style = SKPaintStyle.Fill
        };

        drawContext.Canvas.DrawLine(
            startPoint, endPoint, paint
        );
    }

    private record ContainerSizes(
        int StartX,
        int StartY,
        int EndX,
        int EndY
    )
    {
        public int X => StartX + EndX;
        public int Y => StartY + EndY;
    }
}

public abstract class GapElementCollection : ElementCollection
{

    /// <summary>
    /// Gap between Items
    /// </summary>
    public int Gap { get; set; }
}