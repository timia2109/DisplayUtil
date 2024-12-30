using SkiaSharp;

namespace DisplayUtil.Infrastructure.Providers.Image;

/// <summary>
/// Provides images for other parts the application.
/// </summary>
public interface IImageProvider
{
    /// <summary>
    /// Can this Image Provider resolve the given image name?
    /// </summary>
    /// <param name="imageName">Name of the image</param>
    /// <returns>Can resolve?</returns>
    bool CanResolve(string imageName);

    /// <summary>
    /// Get the image with the given name.
    /// </summary>
    /// <param name="imageName">Name of the image</param>
    /// <returns>The image</returns>
    ValueTask<SKBitmap> GetImageAsync(string imageName);
}