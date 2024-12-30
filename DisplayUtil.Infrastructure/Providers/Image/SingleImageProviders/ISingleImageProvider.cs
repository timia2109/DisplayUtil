using SkiaSharp;

namespace DisplayUtil.Infrastructure.Providers.Image.SingleImageProviders;

/// <summary>
/// Provides a single image.
/// </summary>
public interface ISingleImageProvider
{
    /// <summary>
    /// Creates a single image
    /// </summary>
    /// <returns>The image</returns>
    ValueTask<SKBitmap> GetImageAsync();
}