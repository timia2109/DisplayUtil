using SkiaSharp;

namespace DisplayUtil.Layouting.Utils;

/// <summary>
///     Responsible to Draw an SVG-Icon
/// </summary>
public interface IIconDrawer : IDisposable
{
    /// <summary>
    ///     Draws an Icon to the given Canvas
    /// </summary>
    /// <param name="iconName">Name of icon</param>
    /// <param name="height">Height of icon</param>
    /// <param name="x">X-Coordinate</param>
    /// <param name="y">Y-Coordinate</param>
    /// <param name="canvas">Canvas</param>
    /// <returns>Size of the icon</returns>
    public SKSize? DrawIcon(string iconName, int height, int x, int y,
        SKCanvas canvas);

    /// <summary>
    ///     Draws an Icon to the given Canvas
    /// </summary>
    /// <param name="iconName">Name of icon</param>
    /// <param name="height">Height of icon</param>
    /// <param name="point">Coordinates as SkPoint</param>
    /// <param name="canvas">Canvas</param>
    /// <returns>Size of the icon</returns>
    public SKSize? DrawIcon(string iconName, int height, SKPoint point, SKCanvas canvas);

    /// <summary>
    ///     Calculates the size of the icon
    /// </summary>
    /// <param name="iconName">Name of icon</param>
    /// <param name="height">Heigh of icon</param>
    /// <returns>Size of the icon</returns>
    public SKSize? GetSize(string iconName, int height);
}