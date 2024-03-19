using SkiaSharp;

namespace DisplayUtil.Scenes;

public class ScreenRepository(
    IReadOnlyDictionary<string, IScreenProvider> screenProviders
)
{
    public Task<SKBitmap> GetImageAsync(string screenProviderId)
    {
        if (!screenProviders.TryGetValue(screenProviderId, out var storedScreenProvider))
        {
            throw new Exception($"ScreenProvider with Id {screenProviderId} not found");
        }

        return storedScreenProvider.GetImageAsync();
    }
}