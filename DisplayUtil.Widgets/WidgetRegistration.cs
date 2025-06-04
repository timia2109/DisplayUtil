namespace DisplayUtil.Widgets;

/// <summary>
///     A registration for a widget.
/// </summary>
/// <param name="Id">Name of the Widget</param>
/// <param name="Priority">Priority of this Widget (if them need to be limited)</param>
/// <param name="WidgetGroup">Group of the Widget (for grouping widgets)</param>
public record struct WidgetRegistration(
    string Id,
    int Priority,
    string WidgetGroup = IWidgetRegistry.DefaultWidgetGroup
);