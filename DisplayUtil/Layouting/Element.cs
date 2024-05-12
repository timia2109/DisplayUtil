using DisplayUtil.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using SkiaSharp;

namespace DisplayUtil.Layouting;

public record struct DrawContext(
    SKCanvas Canvas,
    SKSize Size,
    SKPoint StartPoint,
    DrawResources DrawResources
);

/// <summary>
/// Abstract base Element. Elements are designed to be used only one time!
/// </summary>
public abstract class Element : IDisposable
{
    /// <summary>
    /// Caching calculated size
    /// </summary>
    private SKSize? _size = null;

    /// <summary>
    /// Gets the Size of the Element
    /// </summary>
    /// <param name="drawContext">Context</param>
    /// <returns>The Size of this element</returns>
    public SKSize GetSize(DrawContext drawContext)
    {
        if (_size is not null) return _size.Value;

        _size = CalculateSize(drawContext);
        return _size.Value;
    }

    /// <summary>
    /// Calculates the Size. This value gets cached
    /// </summary>
    /// <param name="drawContext">DrawContext</param>
    /// <returns>Size of the Element</returns>
    protected abstract SKSize CalculateSize(DrawContext drawContext);

    /// <summary>
    /// Draws the actual Element
    /// </summary>
    /// <param name="drawContext">Context</param>
    public abstract void Draw(DrawContext drawContext);

    public virtual void Dispose()
    { }
}
