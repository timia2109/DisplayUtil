using DisplayUtil.Home.HomeAssistant;
using DisplayUtil.Home.Utils;
using DisplayUtil.Infrastructure;
using DisplayUtil.Layouting;
using DisplayUtil.Widgets;

namespace DisplayUtil.Home.Widgets;

public class MediaPlayerWidget(
    MediaPlayerService mediaPlayerService,
    ElementBuilderProvider elementBuilderProvider,
    MediaAppIconHelper mediaAppIconHelper,
    string mediaPlayerEntity
) : IWidget
{
    /// <summary>
    /// The min. media duration, when the remaining bar should gets displayed
    /// </summary> 
    private static readonly TimeSpan MinRemainingDuration
        = TimeSpan.FromMinutes(5);

    private bool _mediaContentInitialized = false;
    private MediaContent? _mediaContent;
    private MediaContent? MediaContent
    {
        get
        {
            if (_mediaContentInitialized)
                return _mediaContent;

            _mediaContent = mediaPlayerService.GetMediaContent(mediaPlayerEntity);
            _mediaContentInitialized = true;
            return _mediaContent;
        }
    }

    public bool IsActive => MediaContent is not null;

    public Task<Element> RenderAsync(CancellationToken cancellationToken)
    {
        var content = MediaContent;

        if (content is null)
        {
            return Task.FromResult(
                elementBuilderProvider
                    .VBoxBuilder
                    .WithText("No media playing")
                    .Build()
            );
        }

        var vbox = elementBuilderProvider.VBoxBuilder
            .WithGap(2)
            .WithBorder(b => b.WithY(2))
            .WithPadding(p => p.WithAll(2));

        var contentIconName = content.MediaContentType switch
        {
            MediaContentType.Music => "fa:list-music",
            MediaContentType.Video => "fa:film",
            _ => "fa:tv-music"
        };

        vbox
            .WithIconElement(
                mediaAppIconHelper.GetIconForApp(content.AppName
                    ?? string.Empty),
                content.AppName
            )
            .WithIconElement(
                "fa:user-music",
                content.MediaArtist
            )
            .WithIconElement(
                "fa:album",
                content.MediaAlbumName
            )
            .WithIconElement(
                contentIconName,
                content.MediaTitle
            )
        ;

        // Remaining Bar
        if (content.IsPaused)
            // Paused
            vbox.WithIconElement(
                "fa:circle-pause",
                "Pausiert"
            );
        else
        {
            // Plaining
            vbox.AddFlexbox(f => f
                .WithJustifyContent(JustifyContent.Between)
                .WithIconElement(
                    "fa:timer",
                    content.Duration?.ToString("c")
                )
                .WithIconElement("fa:clock",
                    content.EndTime?.ToLocalTime().ToString("T")
                )
            );

            // Remaining Row
            var remainingText = GetRemainingText(content);
            if (remainingText is not null)
                vbox.WithIconElement(
                    "fa:hourglass-clock",
                    remainingText
                );
        }

        return Task.FromResult(vbox.Build());
    }

    private static string? GetRemainingText(MediaContent content)
    {
        if (content.Duration is null || content.MediaRemaining is null)
            return null;

        if (content.Duration < MinRemainingDuration) return null;

        var remaining = content.MediaRemaining.Value;

        if (remaining.TotalHours >= 1)
            return $"Noch {remaining:hh\\:mm}";

        return $"Noch {remaining:mm} min.";
    }
}