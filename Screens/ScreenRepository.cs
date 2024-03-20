using SkiaSharp;

namespace DisplayUtil.Scenes;

public class ScreenRepository(
    IEnumerable<IScreenProviderSource> screenProviderSources
)
{
    /// <summary>
    /// Search the <see cref="IScreenProvider"/> with it's Id
    /// </summary>
    /// <param name="screenProviderId">Id of the screen provider</param>
    /// <returns>ScreenProvider</returns>
    /// <exception cref="Exception">Not found</exception>
    public IScreenProvider GetScreenProvider(string screenProviderId)
    {
        foreach (var screenProviderSource in screenProviderSources)
        {
            var screenProvider = screenProviderSource
                .GetScreenProvider(screenProviderId);

            if (screenProvider is not null) return screenProvider;
        }

        throw new Exception($"ScreenProvider with Id {screenProviderId} not found");
    }

    public Task<SKBitmap> GetImageAsync(string screenProviderId)
    {
        return GetScreenProvider(screenProviderId).GetImageAsync();
    }
}