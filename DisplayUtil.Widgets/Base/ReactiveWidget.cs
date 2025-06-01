using System.Reactive.Subjects;
using DisplayUtil.Layouting;

namespace DisplayUtil.Widgets.Base;

/// <summary>
///     Helper for widgets that need to be reactive.
/// </summary>
public abstract class ReactiveWidget : IWidget
{
    private readonly BehaviorSubject<bool> _isActiveSubject = new(false);

    public IObserver<bool> ActiveObserver => _isActiveSubject;

    public bool IsActive => _isActiveSubject.Value;

    public abstract Task<Element> RenderAsync(CancellationToken cancellationToken);
}