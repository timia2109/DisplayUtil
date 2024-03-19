using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// Adds an Padding for the Element
/// </summary>
/// <param name="padding">Padding</param>
/// <param name="child">Child Element</param>
public class PaddingElement(Padding padding, Element child) : Element
{
    public override void Draw(DrawContext drawContext)
    {
        var childContext = ChildContext(drawContext);
        child.Draw(childContext);
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        var childContext = ChildContext(drawContext);
        var size = child.GetSize(childContext);
        size.Width += padding.Left + padding.Right;
        size.Height += padding.Top + padding.Bottom;
        return size;
    }

    private DrawContext ChildContext(DrawContext context)
    {
        var point = context.StartPoint;
        point.X += padding.Left;
        point.Y += padding.Top;

        var size = context.Size;
        size.Width -= padding.Left + padding.Right;
        size.Height -= padding.Top + padding.Bottom;

        return new DrawContext(context.Canvas, size, point);
    }
}
