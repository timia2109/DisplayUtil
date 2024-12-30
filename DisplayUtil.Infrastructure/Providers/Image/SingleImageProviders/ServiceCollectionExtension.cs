using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Infrastructure.Providers.Image.SingleImageProviders;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds the given type as a single image provider. The given type will also be added as scoped service
    /// </summary>
    /// <typeparam name="TImageProvider">Type of ImageProvider</typeparam>
    /// <param name="services">Services</param>
    /// <param name="imageName">Name of image</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddSingleImageProvider<TImageProvider>(
        this IServiceCollection services,
        string imageName
    )
        where TImageProvider : class, ISingleImageProvider
    {
        services.AddKeyedScoped<ISingleImageProvider, TImageProvider>(imageName);
        services.TryAddKeyedImageRegistryProvider();
        return services;
    }

    /// <summary>
    /// Adds the <see cref="KeyedImageRegistryProvider"/> as <see cref="IImageProvider"/>
    /// if its not already added
    /// </summary>
    /// <param name="services">Services</param>
    /// <returns>Service Collection</returns>
    public static IServiceCollection TryAddKeyedImageRegistryProvider(
        this IServiceCollection services
    )
    {
        if (services
            .Any(e => e.ImplementationType == typeof(KeyedImageRegistryProvider)))
        {
            return services;
        }

        services.AddScoped<IImageProvider, KeyedImageRegistryProvider>();
        return services;
    }


}