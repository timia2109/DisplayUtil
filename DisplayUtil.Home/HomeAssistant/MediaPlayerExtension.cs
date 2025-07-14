using System.Text.Json;
using Microsoft.Extensions.Options;
using NetDaemon.Client.Settings;
using NetDaemon.HassModel;

namespace DisplayUtil.Home.HomeAssistant;

public class MediaPlayerService(
    IOptions<HomeAssistantSettings> settings,
    IHaContext ctx,
    [FromKeyedServices(HassExtension.JsonKey)]
    JsonSerializerOptions jsonSerializerOptions
)
{
    private const string PlayingState = "playing",
            PausedState = "paused";

    public MediaContent? GetMediaContent(string playerEntity)
    {
        var entity = ctx.GetState(playerEntity);

        if (entity is null || entity.State is null
            || entity.State is not (PlayingState or PausedState))
            return null;

        var attributes = entity.AttributesJson
            ?.Deserialize<MediaPlayerAttributes>(jsonSerializerOptions);

        if (attributes is null)
            return null;

        TimeSpan? duration = attributes.MediaDuration is not null
            ? TimeSpan.FromSeconds((double)attributes.MediaDuration)
            : null;
        DateTimeOffset? startTime = null;
        DateTimeOffset? endTime = null;

        if (
            attributes.MediaPosition is not null
            && attributes.MediaPositionUpdatedAt is not null
            && attributes.MediaDuration is not null
        )
        {
            startTime = attributes.MediaPositionUpdatedAt.Value.AddSeconds(-(double)attributes.MediaPosition);
            endTime = startTime.Value.AddSeconds((double)attributes.MediaDuration);
        }

        Uri? mediaPicture = null;
        if (attributes.EntityPictureLocal is not null)
        {
            var builder = new UriBuilder(settings.Value.Host)
            {
                Scheme = settings.Value.Ssl ? "https" : "http",
                Path = attributes.EntityPictureLocal,
                Port = settings.Value.Port
            };
            mediaPicture = builder.Uri;
        }

        return new MediaContent
        {
            PlayerEntity = playerEntity,
            IsPaused = entity.State is PausedState,
            AppId = attributes.AppId,
            AppName = attributes.AppName,
            VolumeLevel = attributes.VolumeLevel,
            IsVolumeMuted = attributes.IsVolumeMuted,
            MediaPicture = mediaPicture,
            MediaTitle = attributes.MediaTitle,
            Duration = duration,
            StartTime = startTime,
            EndTime = endTime,
            MediaContentType = attributes.MediaContentType ?? MediaContentType.Unknown
        };
    }

}

public enum MediaContentType
{
    Unknown,
    Music,
    Video,
    Image,
    TvShow
}

public record MediaPlayerAttributes
{

    public string? AppId { get; init; }

    public string? AppName { get; init; }

    public MediaContentType? MediaContentType { get; init; }

    public decimal? VolumeLevel { get; init; }

    public bool? IsVolumeMuted { get; init; }

    public string? EntityPictureLocal { get; init; }

    public DateTimeOffset? MediaPositionUpdatedAt { get; init; }

    public decimal? MediaDuration { get; init; }

    public string? MediaTitle { get; init; }

    public decimal? MediaPosition { get; init; }
}

public record MediaContent
{
    public required string PlayerEntity { get; init; }

    public string? AppId { get; init; }

    public string? AppName { get; init; }

    public MediaContentType MediaContentType { get; init; }

    public decimal? VolumeLevel { get; init; }

    public bool? IsVolumeMuted { get; init; }

    public Uri? MediaPicture { get; init; }

    public required bool IsPaused { get; init; }

    public TimeSpan? Duration { get; init; }

    public string? MediaTitle { get; init; }

    public TimeSpan? MediaPosition => StartTime is not null
        ? DateTimeOffset.UtcNow - StartTime
        : null;

    public TimeSpan? MediaRemaining => EndTime is not null
        ? EndTime - DateTimeOffset.UtcNow
        : null;

    public DateTimeOffset? StartTime { get; init; }

    public DateTimeOffset? EndTime { get; init; }
}