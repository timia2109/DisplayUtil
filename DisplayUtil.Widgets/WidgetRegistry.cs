namespace DisplayUtil.Widgets;

/// <summary>
///     Registry for widgets.
/// </summary>
/// <param name="registrations">Registered Widgets (must be sorted for priority descending)</param>
internal class WidgetRegistry(WidgetRegistration[] registrations) : IWidgetRegistry
{
    public IEnumerable<WidgetRegistration> GetRegisteredWidgets()
    {
        return registrations;
    }

    public IEnumerable<WidgetRegistration> GetRegisteredWidgets(string widgetGroup)
    {
        return registrations
            .Where(w => w.WidgetGroup.Equals(widgetGroup, StringComparison.OrdinalIgnoreCase));
    }
}