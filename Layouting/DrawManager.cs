using SkiaSharp;

namespace DisplayUtil.Layouting;

public static class DrawManager
{
    /// <summary>
    /// Draws the element on the given Bitmap
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <param name="element">Element</param>
    public static void Draw(SKBitmap bitmap, Element element)
    {
        var size = new SKSize(bitmap.Width, bitmap.Height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var rootContext = new DrawContext(canvas, size, SKPoint.Empty);

        element.Draw(rootContext);
        canvas.Flush();
    }

    /// <summary>
    /// Draws the element on a new Bitmap with the given Size
    /// </summary>
    /// <param name="size">Expected Size</param>
    /// <param name="element">Current Element</param>
    /// <returns>The Drawed Bitmap</returns>
    public static SKBitmap Draw(SKSize size, Element element)
    {
        var bitmap = new SKBitmap((int)size.Width, (int)size.Height);
        Draw(bitmap, element);
        return bitmap;
    }

}