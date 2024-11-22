using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// Simple TextElement
/// </summary>
/// <param name="content">The Content</param>
/// <param name="paint">Painting Information</param>
public class TextElement(string content, SKPaint paint) : Element
{
    public override void Draw(DrawContext drawContext)
    {
        var point = drawContext.StartPoint;
        point.Y -= paint.FontMetrics.Ascent;

        drawContext.Canvas.DrawText(content, point, paint);
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        var width = paint.MeasureText(content);
        return new SKSize(width, paint.TextSize);
    }

    public override void Dispose()
    {
        base.Dispose();
        paint.Dispose();
    }
}
