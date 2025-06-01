using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Widgets;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddWidgets(this IServiceCollection services)
    {
        services.AddSingleton<IWidgetRegistry, WidgetRegistry>();
        services.AddScoped<IWidgetService, WidgetService>();
        return services;
    }
}