using DisplayUtil.Layouting.Utils;
using DisplayUtil.Providers;
using SkiaSharp;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace DisplayUtil.Utils;

public partial class IconDrawer(
    IconPathProvider iconPathProvider,
    ILogger<IconDrawer> logger
) : IIconDrawer, IDisposable
{
    private readonly Dictionary<CacheKey, CacheEntry> _cache = new();
    private readonly ILogger _logger = logger;

    public SKSize? DrawIcon(string iconName, int height, int x, int y,
        SKCanvas canvas)
    {
        var icon = GetIcon(iconName, height);
        if (icon == null) return null;

        canvas.DrawImage(icon.Image, x, y);
        return icon.Size;
    }

    public SKSize? DrawIcon(string iconName, int height, SKPoint point, SKCanvas canvas)
    {
        var icon = GetIcon(iconName, height);
        if (icon == null) return null;

        canvas.DrawImage(icon.Image, point);
        return icon.Size;
    }

    public SKSize? GetSize(string iconName, int height)
    {
        var icon = GetIcon(iconName, height);
        if (icon == null) return null;

        return icon.Size;
    }

    public void Dispose()
    {
        foreach (var entry in _cache.Values) entry.Dispose();

        _cache.Clear();
    }

    private CacheEntry? GetIcon(string iconName, int height)
    {
        var key = new CacheKey(iconName, height);

        if (_cache.TryGetValue(key, out var icon)) return icon;

        icon = CreateIcon(iconName, height);
        if (icon == null) return null;

        _cache.Add(key, icon);
        return icon;
    }

    private CacheEntry? CreateIcon(string iconName, int height)
    {
        LogCreating(iconName, height);
        var iconPath = iconPathProvider.ResolvePath(iconName);

        if (!File.Exists(iconPath))
        {
            LogFileNotFound(iconName);
            return null;
        }

        using var stream = File.OpenRead(iconPath);

        var svgImage = new SKSvg();
        svgImage.Load(stream);

        var info = svgImage.CanvasSize;
        var heightFactor = height / info.Height;
        var desiredSize = new SKSize(heightFactor * info.Width, height);

        // Draw to Bitmap
        var imageInfo = new SKImageInfo((int)desiredSize.Width, (int)desiredSize.Height);
        using var surface = SKSurface.Create(imageInfo);
        using var tempCanvas = surface.Canvas;

        // calculate the scaling need to fit to screen
        var scaleX = desiredSize.Width / svgImage.Picture.CullRect.Width;
        var scaleY = desiredSize.Height / svgImage.Picture.CullRect.Height;
        var matrix = SKMatrix.CreateScale(scaleX, scaleY);

        // draw the svg
        tempCanvas.Clear(SKColors.Transparent);
        tempCanvas.DrawPicture(svgImage.Picture, ref matrix);
        tempCanvas.Flush();

        var data = surface.Snapshot();
        return new CacheEntry(data, desiredSize);
    }

    [LoggerMessage(LogLevel.Warning, "Icon {iconName} not found!")]
    private partial void LogFileNotFound(string iconName);

    [LoggerMessage(LogLevel.Debug, "Create Icon {iconName} with height {height}")]
    private partial void LogCreating(string iconName, int height);

    private record CacheKey(string IconId, int Height);

    private record CacheEntry(SKImage Image, SKSize Size) : IDisposable
    {
        public void Dispose()
        {
            Image.Dispose();
        }
    }
}