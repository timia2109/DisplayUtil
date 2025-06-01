namespace DisplayUtil.Widgets;

internal class WidgetRegistry : IWidgetRegistry
{
    private readonly Dictionary<string, WidgetRegistration> _widgets = new();

    public void RegisterWidget<TWidget>(string name, string? serviceKey = null,
        int priority = IWidgetRegistry.DefaultPriority) where TWidget : IWidget
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Widget name cannot be null or empty.", nameof(name));
        if (_widgets.ContainsKey(name))
            throw new InvalidOperationException($"A widget with the name '{name}' is already registered.");

        _widgets[name] = new WidgetRegistration(name, typeof(TWidget), serviceKey, priority);
    }

    public IEnumerable<WidgetRegistration> GetRegisteredWidgets() => _widgets.Values
        .OrderByDescending(w => w.Priority);
}