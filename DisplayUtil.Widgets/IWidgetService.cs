using DisplayUtil.Layouting;

namespace DisplayUtil.Widgets;

public record WidgetEntity(string Name, IWidget Widget, int Priority, string WidgetGroup)
{
    public Task<Element> RenderAsync(CancellationToken cancellationToken = default)
    {
        return Widget.RenderAsync(cancellationToken);
    }
}

public interface IWidgetService
{
    /// <summary>
    ///     Get all widgets registered in the system.
    /// </summary>
    /// <returns>Enumerable with WidgetEntities</returns>
    IEnumerable<WidgetEntity> GetAllWidgets();

    /// <summary>
    ///     Get all active widgets registered in the system.
    /// </summary>
    /// <returns>Enumerable with WidgetEntities</returns>
    IEnumerable<WidgetEntity> GetActiveWidgets();

    /// <summary>
    ///     Get Active Widgets for a specific widget group.
    /// </summary>
    /// <param name="widgetGroup">Affected Widget Group</param>
    /// <returns>Widget Entities</returns>
    IEnumerable<WidgetEntity> GetActiveWidgets(string widgetGroup);
}