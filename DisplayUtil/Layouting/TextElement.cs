using SkiaSharp;

namespace DisplayUtil.Layouting;

/// <summary>
/// Simple TextElement
/// </summary>
public class TextElement : Element
{
    /// <summary>
    /// The Content
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// Painting Information
    /// </summary>
    public required SKPaint Paint { get; set; }

    public override void Draw(DrawContext drawContext)
    {
        var point = drawContext.StartPoint;
        point.Y -= Paint.FontMetrics.Ascent;

        drawContext.Canvas.DrawText(Content, point, Paint);
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        var width = Paint.MeasureText(Content);
        return new SKSize(width, Paint.TextSize);
    }

    public override void Dispose()
    {
        base.Dispose();
        Paint.Dispose();
    }
}
