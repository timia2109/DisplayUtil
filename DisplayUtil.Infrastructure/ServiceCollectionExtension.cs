using DisplayUtil.Building;
using DisplayUtil.Building.Font;
using DisplayUtil.Layouting.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Infrastructure;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds a <see cref="ScreenBuilder"/> to the dependency injection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureDefaultDefinition"></param>
    /// <returns></returns>
    public static IServiceCollection AddScreenBuilder(
        this IServiceCollection services,
        Action<DefaultDefinitionBuilder> configureDefaultDefinition
    )
    {
        var builder = new DefaultDefinitionBuilder();
        configureDefaultDefinition(builder);

        services.AddTransient(s =>
        {
            var fontProvider = s.GetRequiredService<IFontProvider>();
            var iconDrawer = s.GetRequiredService<IIconDrawer>();
            var defaultDefinition = builder.DefaultDefinition;
            return new ScreenBuilder(fontProvider, iconDrawer, defaultDefinition);
        });

        return services;
    }

}