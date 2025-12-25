using DisplayUtil.Building.Font;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Infrastructure.Providers.Font;

public static class IServiceCollectionExtension
{
    /// <summary>
    ///     Adds the Font Provider
    /// </summary>
    /// <param name="services">Services</param>
    /// <param name="configure">Configure action</param>
    /// <returns>Service Collection</returns>
    public static IServiceCollection AddFontProvider(
        this IServiceCollection services,
        Action<FontProviderBuilder> configure
    )
    {
        var fontBuilder = new FontProviderBuilder();
        configure(fontBuilder);

        services.AddSingleton<IFontProvider>(s => fontBuilder.Build(s));
        return services;
    }

    public static IServiceCollection AddFontProvider(
        this IServiceCollection services,
        IConfiguration fontConfiguration
    )
    {
        return AddFontProvider(services,
            fontConfiguration.Get<FontConfiguration>()
            ?? throw new Exception("Font configuration is missing")
        );
    }

    public static IServiceCollection AddFontProvider(
        this IServiceCollection services,
        FontConfiguration fontConfiguration
    )
    {
        return AddFontProvider(services, fontConfiguration.Apply);
    }
}