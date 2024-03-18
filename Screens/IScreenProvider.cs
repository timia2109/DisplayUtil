using SkiaSharp;

namespace DisplayUtil.Scenes;

public interface IScreenProvider
{
    /// <summary>
    /// Returns the current Image from the ScreenProvider
    /// </summary>
    /// <returns>Image</returns>
    Task<SKBitmap> GetImageAsync();

}