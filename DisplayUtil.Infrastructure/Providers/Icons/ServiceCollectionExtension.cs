using DisplayUtil.Layouting.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DisplayUtil.Infrastructure.Providers.Icons;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds a directory icon provider to the service collection.
    /// Prefix is used to identify the icons provided by this provider.
    /// Icon Name will be "prefix:iconName".
    /// The <see cref="IIconDrawer"/> will also be added to the service collection.
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="iconPrefix">Identifying Prefix</param>
    /// <param name="folderPath">Path of Icons</param>
    /// <returns>Service Collection</returns>
    public static IServiceCollection AddFolderIconProvider(
        this IServiceCollection services,
        string iconPrefix,
        string folderPath
    )
    {
        services.TryAddIconDrawer();
        services.AddSingleton<IIconProvider>(
            new DirectoryIconProvider(iconPrefix, folderPath)
        );
        return services;
    }

    public static IServiceCollection AddFolderIconProvider(
        this IServiceCollection services,
        IconConfiguration iconConfiguration
    )
    {
        services.TryAddIconDrawer();

        foreach (var (iconPrefix, folderPath) in iconConfiguration.Icons)
        {
            services.AddSingleton<IIconProvider>(
                new DirectoryIconProvider(iconPrefix, folderPath)
            );
        }

        return services;
    }

    public static IServiceCollection AddFolderIconProvider(
        this IServiceCollection services,
        IConfiguration iconConfiguration
    )
    {
        return AddFolderIconProvider(
            services,
            iconConfiguration.Get<IconConfiguration>()
                ?? throw new Exception("Icon configuration is missing")
        );
    }

    /// <summary>
    /// Try to add the standard icon drawer to the service collection.
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <returns>Service Collection</returns>
    public static IServiceCollection TryAddIconDrawer(
        this IServiceCollection services
    )
    {
        services.TryAddScoped<IIconDrawer, StdIconDrawer>();
        return services;
    }
}
