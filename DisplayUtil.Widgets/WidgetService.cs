using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.Widgets;

internal class WidgetService(IServiceProvider serviceProvider, IWidgetRegistry widgetRegistry) : IWidgetService
{
    public IEnumerable<WidgetEntity> GetAllWidgets()
    {
        return widgetRegistry.GetRegisteredWidgets()
            .Select(GetEntity);
    }

    public IEnumerable<WidgetEntity> GetActiveWidgets()
    {
        return GetAllWidgets()
            .Where(w => w.Widget.IsActive);
    }

    public IEnumerable<WidgetEntity> GetActiveWidgets(string widgetGroup)
    {
        return GetActiveWidgets()
            .Where(w => w.WidgetGroup.Equals(widgetGroup, StringComparison.OrdinalIgnoreCase));
    }

    private WidgetEntity GetEntity(WidgetRegistration registration)
    {
        return new WidgetEntity(registration.Id, GetWidget(registration), registration.Priority,
            registration.WidgetGroup);
    }

    private IWidget GetWidget(WidgetRegistration registration)
    {
        return serviceProvider.GetRequiredKeyedService<IWidget>(
            registration.Id);
    }
}