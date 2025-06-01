using DisplayUtil.Layouting;

namespace DisplayUtil.Widgets;

/// <summary>
///     A widget is a component that can be rendered on a screen.
/// </summary>
public interface IWidget
{
    /// <summary>
    ///     Is this widget active?
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    ///     Renders the widget to an Element.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task with Element</returns>
    Task<Element> RenderAsync(CancellationToken cancellationToken);
}