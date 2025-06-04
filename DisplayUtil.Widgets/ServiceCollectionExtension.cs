using DisplayUtil.Widgets.Building;
using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Widgets;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddWidgets(
        this IServiceCollection services,
        Action<IWidgetRegistryBuilder> configure
    )
    {
        var builder = new WidgetRegistryBuilder(services);
        configure(builder);
        builder.Build();

        services.AddScoped<IWidgetService, WidgetService>();
        return services;
    }
}