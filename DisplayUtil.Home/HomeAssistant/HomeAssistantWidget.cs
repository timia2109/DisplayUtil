using DisplayUtil.Widgets.Base;
using NetDaemon.HassModel;

namespace DisplayUtil.Home.HomeAssistant;

public abstract class HomeAssistantWidget(IHaContext ctx) : ReactiveWidget
{
    protected IHaContext Context { get; } = ctx;

    public void OnInitialize()
    {
        Subscribe(Context).Subscribe(
            ActiveObserver.OnNext,
            ActiveObserver.OnError,
            ActiveObserver.OnCompleted
        );
    }

    protected abstract IObservable<bool> Subscribe(IHaContext context);
}