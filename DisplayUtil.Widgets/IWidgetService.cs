using DisplayUtil.Layouting;

namespace DisplayUtil.Widgets;

public record WidgetEntity(string Name, IWidget Widget, int Priority)
{
    public Task<Element> RenderAsync(CancellationToken cancellationToken = default) =>
        Widget.RenderAsync(cancellationToken);
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
}