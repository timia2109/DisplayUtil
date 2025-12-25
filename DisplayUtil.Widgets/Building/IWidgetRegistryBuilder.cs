namespace DisplayUtil.Widgets.Building;

public interface IWidgetRegistryBuilder
{
    /// <summary>
    ///     Registers a widget to the registry and to the Dependency Injection.
    /// </summary>
    /// <typeparam name="TWidget">Type of widget</typeparam>
    /// <param name="widgetId">Id of Widget</param>
    /// <param name="priority">Priority of this Widget (if them need to be limited)</param>
    /// <param name="widgetGroup">Group of the Widget (for grouping widgets)</param>
    void RegisterWidget<TWidget>(
        string widgetId,
        int priority = IWidgetRegistry.DefaultPriority,
        string widgetGroup = IWidgetRegistry.DefaultWidgetGroup
    ) where TWidget : class, IWidget;

    /// <summary>
    ///     Registers a widget to the registry and to the Dependency Injection.
    /// </summary>
    /// <param name="widgetId">Id of Widget</param>
    /// <param name="widgetFactory">Factory to create the widget</param>
    /// <param name="priority">Priority of this Widget (if them need to be limited)</param>
    /// <param name="widgetGroup">Group of the Widget (for grouping widgets)</param>
    void RegisterWidget(
        string widgetId,
        Func<IServiceProvider, object?, IWidget> widgetFactory,
        int priority = IWidgetRegistry.DefaultPriority,
        string widgetGroup = IWidgetRegistry.DefaultWidgetGroup
    );
}