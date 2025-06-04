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
        new(registration.Id, GetWidget(registration), registration.Priority, registration.WidgetGroup);

    private IWidget GetWidget(WidgetRegistration registration)
    {
        return serviceProvider.GetRequiredKeyedService<IWidget>(
            registration.Id);
    }

    public IEnumerable<WidgetEntity> GetActiveWidgets(string widgetGroup)
        => GetActiveWidgets()
            .Where(w => w.WidgetGroup.Equals(widgetGroup, StringComparison.OrdinalIgnoreCase));
}