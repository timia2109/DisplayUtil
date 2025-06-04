using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DisplayUtil.Widgets.Building;

internal class WidgetRegistryBuilder(IServiceCollection services) : IWidgetRegistryBuilder
{
    private readonly Dictionary<string, WidgetRegistration> _registrations = new();

    public void RegisterWidget<TWidget>(
        string widgetId,
        int priority = IWidgetRegistry.DefaultPriority,
        string widgetGroup = IWidgetRegistry.DefaultWidgetGroup
    ) where TWidget : class, IWidget
    {
        if (_registrations.ContainsKey(widgetId))
            throw new ArgumentException($"A widget with the id '{widgetId}' is already registered.");

        var registration = new WidgetRegistration(
            widgetId,
            priority,
            widgetGroup
        );

        services.TryAddKeyedScoped<IWidget, TWidget>(widgetId);
        _registrations[widgetId] = registration;
    }

    public void RegisterWidget(
        string widgetId,
        Func<IServiceProvider, object?, IWidget> widgetFactory,
        int priority = 100,
        string widgetGroup = "default"
    )
    {
        if (_registrations.ContainsKey(widgetId))
            throw new ArgumentException($"A widget with the id '{widgetId}' is already registered.");

        var registration = new WidgetRegistration(
            widgetId,
            priority,
            widgetGroup
        );

        services.TryAddKeyedScoped<IWidget>(widgetId, widgetFactory);
        _registrations[widgetId] = registration;
    }

    internal void Build()
    {
        var widgets = _registrations.Values
            .OrderByDescending(r => r.Priority)
            .ToArray();

        services.AddSingleton<IWidgetRegistry>(
            new WidgetRegistry(widgets)
        );
    }

}