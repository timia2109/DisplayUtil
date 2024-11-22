using SkiaSharp;

namespace DisplayUtil.Layouting.Utils;

public interface IIconDrawer : IDisposable
{
    public SKSize? DrawIcon(string iconName, int height, int x, int y,
            SKCanvas canvas);

    public SKSize? DrawIcon(string iconName, int height, SKPoint point, SKCanvas canvas);

    public SKSize? GetSize(string iconName, int height);

}