using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Infrastructure.Providers.Image;

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

    /// <summary>
    /// Maps a route to get the image preview
    /// </summary>
    /// <param name="app">WebApp</param>
    /// <param name="route">URI to preview page</param>
    /// <returns>Web App</returns>
    public static WebApplication MapImagePreview(
        this WebApplication app,
        string route = "/preview/{imageId}"
    )
    {
        app.MapGet(route, async (string imageId, ImageResolver imageResolver,
            HttpRequest request) =>
        {
            var image = await imageResolver.GetImageAsync(imageId);
            if (image == null)
            {
                return Results.NotFound();
            }

            var acceptHeader = request.Headers.Accept;
            var format = SkiaSharp.SKEncodedImageFormat.Png;
            var mimeType = "image/png";

            if (acceptHeader.Any(a => a?.Contains("image/jpeg") ?? false))
            {
                format = SkiaSharp.SKEncodedImageFormat.Jpeg;
                mimeType = "image/jpeg";
            }

            var data = image.Encode(format, 100);
            return Results.File(data.ToArray(), mimeType);
        })
        .WithName("Preview Image")
        .WithOpenApi();

        return app;
    }


}