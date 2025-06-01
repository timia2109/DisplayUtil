namespace DisplayUtil.Widgets;

/// <summary>
///     A registration for a widget.
/// </summary>
/// <param name="Name">Name of the Widget</param>
/// <param name="WidgetType">Type of the Widget</param>
/// <param name="ServiceKey">Optional service key for the widget</param>
/// <param name="Priority">Priority of this Widget (if them need to be limited)</param>
public record struct WidgetRegistration(string Name, Type WidgetType, string? ServiceKey, int Priority);

public interface IWidgetRegistry
{
    public const int DefaultPriority = 100;

    /// <summary>
    ///     Registers a widget with the registry.
    /// </summary>
    /// <typeparam name="TWidget">Type of widget</typeparam>
    /// <param name="name">Name of Widget</param>
    /// <param name="serviceKey">Service Key (if any)</param>
    /// ///
    /// <param name="priority">Priority of this Widget (if them need to be limited)</param>
    void RegisterWidget<TWidget>(string name, string? serviceKey = null, int priority = DefaultPriority)
        where TWidget : IWidget;

    /// <summary>
    ///     Get all registered widgets.
    /// </summary>
    /// <returns>Enumerable with Widgets</returns>
    IEnumerable<WidgetRegistration> GetRegisteredWidgets();
}