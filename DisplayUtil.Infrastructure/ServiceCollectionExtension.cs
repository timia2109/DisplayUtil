using DisplayUtil.Building;
using DisplayUtil.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Infrastructure;

public static class ServiceCollectionExtension
{
    /// <summary>
    ///     Adds a <see cref="ScreenBuilder" /> to the dependency injection
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

        services.AddSingleton(new DefaultDefinitionProvider(
            builder.DefaultDefinition));

        services.AddScoped<ElementBuilderProvider>();

        services.AddTransient(s =>
        {
            var elementBuilderProvider = s.GetRequiredService<ElementBuilderProvider>();
            return elementBuilderProvider.ScreenBuilder;
        });

        return services;
    }
}