using SkiaSharp;

namespace DisplayUtil.Infrastructure.Providers.Image;

/// <summary>
/// Resolves images from all registred <see cref="IImageProvider"/>s
/// </summary>
public sealed class ImageResolver(
    IEnumerable<IImageProvider> imageProviders
)
{
    /// <summary>
    /// Gets the image (or null if not found)
    /// </summary>
    /// <param name="imageName">Name of the image</param>
    /// <returns>Image or null</returns>
    public async ValueTask<SKBitmap?> GetImageAsync(string imageName)
    {
        foreach (var provider in imageProviders)
        {
            if (provider.CanResolve(imageName))
            {
                return await provider.GetImageAsync(imageName);
            }
        }

        return null;
    }
}