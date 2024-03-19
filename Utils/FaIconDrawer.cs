using SkiaSharp;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace DisplayUtil.Utils;

public class FaIconDrawer
{

    public static SKSize? DrawIcon(string iconName, int width, int x, int y,
        SKCanvas canvas)
    {
        var iconPath = $"./Resources/svgs/light/{iconName}.svg";

        if (!File.Exists(iconPath))
            return null;

        using var stream = File.OpenRead(iconPath);

        var svgImage = new SKSvg();
        svgImage.Load(stream);

        var info = svgImage.CanvasSize;
        var widthFactor = width / info.Width;
        var desiredSize = new SKSize(width, widthFactor * info.Height);

        // Draw to Bitmap
        var imageInfo = new SKImageInfo((int)desiredSize.Width, (int)desiredSize.Height);
        using var surface = SKSurface.Create(imageInfo);
        using var tempCanvas = surface.Canvas;

        // calculate the scaling need to fit to screen
        var scaleX = desiredSize.Width / svgImage.Picture.CullRect.Width;
        var scaleY = desiredSize.Height / svgImage.Picture.CullRect.Height;
        var matrix = SKMatrix.CreateScale((float)scaleX, (float)scaleY);

        // draw the svg
        tempCanvas.Clear(SKColors.Transparent);
        tempCanvas.DrawPicture(svgImage.Picture, ref matrix);
        tempCanvas.Flush();

        using var data = surface.Snapshot();

        // Draw to main canvas
        canvas.DrawImage(data, x, y);

        return desiredSize;
    }
}