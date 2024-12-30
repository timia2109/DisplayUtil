using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;

namespace DisplayUtil.Infrastructure.Providers.Image.SingleImageProviders;

public class KeyedImageRegistryProvider(IServiceProvider serviceProvider) : IImageProvider
{
    public bool CanResolve(string imageName)
    {
        // TODO: Optimize
        return serviceProvider.GetKeyedService<IServiceProvider>(imageName)
            != null;
    }

    public ValueTask<SKBitmap> GetImageAsync(string imageName)
    {
        return serviceProvider
            .GetRequiredKeyedService<ISingleImageProvider>(imageName)
            .GetImageAsync();
    }
}