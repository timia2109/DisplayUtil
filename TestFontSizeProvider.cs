using DisplayUtil.Scenes;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil;

internal class TestFontSizeProvider(FaIconDrawer iconDrawer) : IScreenProvider
{
    public Task<SKBitmap> GetImageAsync()
    {
        var bitmap = new SKBitmap(64, 32);
        var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var productSans = SKTypeface.FromFile("./Resources/Roboto-Medium.ttf");

        var paint = new SKPaint
        {
            IsAntialias = true,
            TextSize = 32,
            TextAlign = SKTextAlign.Left,
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            Typeface = productSans
        };
        canvas.DrawText("A", 0, 32, paint);

        iconDrawer.DrawIcon("couch", 32, new SKPoint(32, 0), canvas);

        return Task.FromResult(bitmap);
    }
}