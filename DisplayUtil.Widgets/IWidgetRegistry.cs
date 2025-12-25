namespace DisplayUtil.Widgets;

public interface IWidgetRegistry
{
    public const int DefaultPriority = 100;
    public const string DefaultWidgetGroup = "default";

    /// <summary>
    ///     Get all registered widgets.
    /// </summary>
    /// <returns>Enumerable with Widgets</returns>
    IEnumerable<WidgetRegistration> GetRegisteredWidgets();

    /// <summary>
    ///     Get all registered widgets for a specific widget group.
    /// </summary>
    /// <param name="widgetGroup">Affected Widget Group</param>
    /// <returns>Enumerable with Widget Registrations</returns>
    IEnumerable<WidgetRegistration> GetRegisteredWidgets(string widgetGroup);
}