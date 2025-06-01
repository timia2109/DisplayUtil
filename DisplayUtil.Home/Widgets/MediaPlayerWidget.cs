using System.Reactive.Linq;
using DisplayUtil.Home.HomeAssistant;
using DisplayUtil.Layouting;
using NetDaemon.HassModel;

namespace DisplayUtil.Home.Widgets;

public class MediaPlayerWidget(IHaContext ctx, string mediaPlayerEntity) : HomeAssistantWidget(ctx)
{
    public override Task<Element> RenderAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    protected override IObservable<bool> Subscribe(IHaContext context) => context.Entity(mediaPlayerEntity)
        .StateChanges()
        .Select(e => e.New?.State is "playing" or "paused");
}