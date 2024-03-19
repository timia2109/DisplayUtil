using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// Adds a Border to an Item
/// </summary>
/// <param name="border">Border</param>
/// <param name="child">Child</param>
public class BorderElement(Padding border, Element child) : Element
{
    public override void Draw(DrawContext drawContext)
    {
        var size = GetSize(drawContext);

        var topEnd = drawContext.StartPoint;
        topEnd.X += size.Width;

        var leftEnd = drawContext.StartPoint;
        leftEnd.Y += size.Height;

        var rightStart = drawContext.StartPoint;
        rightStart.X += size.Width;

        var rightEnd = rightStart;
        rightEnd.Y += size.Height;

        var bottomStart = drawContext.StartPoint;
        bottomStart.Y += size.Height;

        var bottomEnd = bottomStart;
        bottomEnd.X += size.Width;

        DrawLine(drawContext, drawContext.StartPoint, topEnd, border.Top);
        DrawLine(drawContext, drawContext.StartPoint, leftEnd, border.Left);
        DrawLine(drawContext, bottomStart, bottomEnd, border.Bottom);
        DrawLine(drawContext, rightStart, rightEnd, border.Right);

        child.Draw(drawContext);
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        return child.GetSize(drawContext);
    }

    private void DrawLine(DrawContext drawContext, SKPoint startPoint,
        SKPoint endPoint, int width)
    {
        if (width == 0) return;

        var paint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = width,
            Style = SKPaintStyle.Fill
        };

        drawContext.Canvas.DrawLine(
            startPoint, endPoint, paint
        );
    }
}