using DisplayUtil.Scenes;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil;

internal class TestProvider(FaIconDrawer iconDrawer) : IScreenProvider
{
    private string[] _iconsList = GetIconsList();

    private static string[] GetIconsList()
    {
        var iconsDir = new DirectoryInfo("./Resources/svgs/light");
        return iconsDir.GetFiles()
            .OrderByDescending(e => e.Name)
            .Select(e => Path.GetFileNameWithoutExtension(e.FullName))
            .ToArray();
    }

    public Task<SKBitmap> GetImageAsync()
    {
        var bitmap = new SKBitmap(480, 800);
        var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.Transparent);

        // Stress Test Icons
        int maxHeight = 0;
        int i = 0;
        SKSize? lastSize = new SKSize(0, 0);

        for (int height = 0; height < bitmap.Height; height += maxHeight + 5)
        {
            maxHeight = 0;
            for (int width = 0; width < bitmap.Width; width += (int)lastSize.Value.Width + 5)
            {
                var iconName = _iconsList[i++];
                lastSize = iconDrawer.DrawIcon(iconName, 16, width, height, canvas);
                maxHeight = Math.Max(maxHeight, (int)lastSize.Value.Height);
            }
        }

        var productSans = SKTypeface.FromFile("./Resources/ProductSansRegular.ttf");

        var paint = new SKPaint
        {
            IsAntialias = true,
            TextSize = 50,
            TextAlign = SKTextAlign.Center,
            Color = 0xFF3498DB,
            Style = SKPaintStyle.Fill,
            Typeface = productSans
        };
        canvas.DrawText("TestImage", 128, 128 + (paint.TextSize / 2), paint);


        paint = new SKPaint
        {
            TextSize = 50,
            TextAlign = SKTextAlign.Center,
            Color = 0xFF0000FF,
            Style = SKPaintStyle.Fill,
            Typeface = productSans
        };

        canvas.DrawText("TimItt", 256, 300, paint);


        return Task.FromResult(bitmap);
    }
}