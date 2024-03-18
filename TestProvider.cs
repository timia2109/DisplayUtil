using DisplayUtil.Scenes;
using SkiaSharp;

namespace DisplayUtil;

internal class TestProvider : IScreenProvider
{
    public Task<SKBitmap> GetImageAsync()
    {
        var bitmap = new SKBitmap(480, 800);
        var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.Transparent);

        var paint = new SKPaint
        {
            IsAntialias = true,                               // smooth text
            TextSize = 50,                                    // 50px high text
            TextAlign = SKTextAlign.Center,                   // center the text
            Color = 0xFF3498DB,                               // Xamarin light blue text
            Style = SKPaintStyle.Fill,                        // solid text
            Typeface = SKTypeface.FromFamilyName("Arial")
        };

        canvas.DrawText("TestImage", 128, 128 + (paint.TextSize / 2), paint);

        return Task.FromResult(bitmap);
    }
}