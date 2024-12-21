using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Infrastructure.Providers.Font;

public class FontBuilder
{

}

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddFonts(this IServiceCollection services)
    {
        services.AddSingleton<IFontProvider, StaticFileFontProvider>();
        return services;
    }
}