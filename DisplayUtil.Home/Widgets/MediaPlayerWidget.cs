using System.Reactive.Linq;
using DisplayUtil.Home.HomeAssistant;
using DisplayUtil.Home.Utils;
using DisplayUtil.Infrastructure;
using DisplayUtil.Layouting;
using DisplayUtil.Widgets;
using NetDaemon.HassModel;
using NetDaemon.HassModel.Entities;

namespace DisplayUtil.Home.Widgets;

public class MediaPlayerWidget(IHaContext ctx,
    ElementBuilderProvider elementBuilderProvider,
    string mediaPlayerEntity
) : IWidget
{
    private const string PlayingState = "playing",
        PausedState = "paused";

    private Entity PlayerEntity => ctx.Entity(mediaPlayerEntity);

    public bool IsActive => PlayerEntity.State is PlayingState or PausedState;

    public Task<Element> RenderAsync(CancellationToken cancellationToken)
    {
        var vbox = elementBuilderProvider.VBoxBuilder
            .WithGap(2)
            .WithBorder(b => b.WithY(2))
            .WithPadding(p => p.WithAll(2));

        vbox.WithIconElement(
            "fa:airplay",
            "TODO: APPNAME"
        );

        return Task.FromResult(vbox.Build());
    }
}