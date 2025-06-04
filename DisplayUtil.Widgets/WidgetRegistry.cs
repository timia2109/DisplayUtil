namespace DisplayUtil.Widgets;

/// <summary>
/// Registry for widgets.
/// </summary>
/// <param name="registrations">Registered Widgets (must be sorted for priority descending)</param> 
internal class WidgetRegistry(WidgetRegistration[] registrations) : IWidgetRegistry
{
    public IEnumerable<WidgetRegistration> GetRegisteredWidgets() => registrations;

    public IEnumerable<WidgetRegistration> GetRegisteredWidgets(string widgetGroup)
        => registrations
            .Where(w => w.WidgetGroup.Equals(widgetGroup, StringComparison.OrdinalIgnoreCase));
}