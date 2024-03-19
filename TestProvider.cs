using DisplayUtil.Scenes;
using DisplayUtil.Utils;
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

        // Test Draw Image
        /*var img = SKBitmap.Decode(@"X:\Media\Favicon.png");
        img = img.Resize(new SKSizeI(64, 64), SKFilterQuality.High);
        canvas.DrawBitmap(img, 0, 0);*/

        // Test Draw FontAwesome (SVG)
        var iconSize = FaIconDrawer.DrawIcon("rocket", 32, 10, 200, canvas);

        return Task.FromResult(bitmap);
    }
}