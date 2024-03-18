using SkiaSharp;

namespace DisplayUtil.Scenes;

public class ScreenRepository(
    IReadOnlyDictionary<string, Type> screenProviders,
    IServiceProvider serviceProvider
)
{
    public Task<SKBitmap> GetImageAsync(string screenProviderId)
    {
        if (!screenProviders.TryGetValue(screenProviderId, out var screenProviderType))
        {
            throw new Exception($"ScreenProvider with Id {screenProviderId} not found");
        }

        var screenProvider = (IScreenProvider)
            serviceProvider.GetRequiredService(screenProviderType);

        return screenProvider.GetImageAsync();
    }
}