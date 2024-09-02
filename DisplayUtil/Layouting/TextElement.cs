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
    /// Font Id of this Text
    /// </summary>
    public required string Font { get; set; }

    /// <summary>
    /// Size ot the text
    /// </summary>
    public required int Size { get; set; }

    /// <summary>
    /// Painting Information
    /// </summary>
    private SKPaint? _paint;

    public override void Draw(DrawContext drawContext)
    {
        var point = drawContext.StartPoint;
        point.Y -= _paint!.FontMetrics.Ascent;

        drawContext.Canvas.DrawText(Content, point, _paint);
    }

    protected override SKSize CalculateSize(DrawContext drawContext)
    {
        _paint = new SKPaint
        {
            IsAntialias = true,
            TextSize = Size,
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            Typeface = drawContext.DrawResources.FontProvider.GetFont(Font)
        };

        var width = _paint.MeasureText(Content);
        return new SKSize(width, _paint.TextSize);
    }

    public override void Dispose()
    {
        base.Dispose();
        _paint?.Dispose();
    }
}
