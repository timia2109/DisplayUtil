using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Widgets;

internal class WidgetService(IServiceProvider serviceProvider, IWidgetRegistry widgetRegistry) : IWidgetService
{
    public IEnumerable<WidgetEntity> GetAllWidgets() =>
        widgetRegistry.GetRegisteredWidgets()
            .Select(GetEntity);

    public IEnumerable<WidgetEntity> GetActiveWidgets()
    {
        return GetAllWidgets()
            .Where(w => w.Widget.IsActive);
    }

    private WidgetEntity GetEntity(WidgetRegistration registration) =>
        new(registration.Name, GetWidget(registration), registration.Priority);

    private IWidget GetWidget(WidgetRegistration registration)
    {
        if (registration.ServiceKey != null)
            return (IWidget)serviceProvider.GetRequiredKeyedService(registration.WidgetType, registration.ServiceKey);

        return (IWidget)serviceProvider.GetRequiredService(registration.WidgetType);
    }
}