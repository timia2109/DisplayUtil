using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Options;
using NetDaemon.Client.Settings;
using NetDaemon.HassModel;

namespace DisplayUtil.Home.HomeAssistant;

public class MediaPlayerService(
    IOptions<HomeAssistantSettings> settings,
    IHaContext ctx,
    HassUtil hassUtil,
    [FromKeyedServices(HassExtension.JsonKey)]
    JsonSerializerOptions jsonSerializerOptions
)
{
    private const string PlayingState = "playing",
            PausedState = "paused",
            UnavailableState = "unavailable";

    /// <summary>
    /// Evaluates the special handling for my tv show entities
    /// </summary>
    /// <param name="playerEntity">Affected Player</param>
    /// <returns>The Media Content information for the TV show (or null if there isn't a show)</returns>
    private TvContent? GetTvShowContent(string playerEntity)
    {
        var playerDomainPos = playerEntity.IndexOf('.');
        var playerName = playerEntity[(playerDomainPos + 1)..];
        var showState = $"sensor.{playerName}_show";

        var showName = hassUtil.GetState(showState);
        if (showName is UnavailableState or null) return null;

        var showStart = hassUtil.GetDateTime($"{showState}_start");
        var showEnd = hassUtil.GetDateTime($"{showState}_end");

        var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
        return new TvContent
        {
            ShowName = showName,
            ShowStart = new DateTimeOffset(showStart!.Value, offset),
            ShowEnd = new DateTimeOffset(showEnd!.Value, offset)
        };
    }

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

        // Try to get TvShow
        var tvShowInfo = GetTvShowContent(playerEntity);
        if (tvShowInfo is not null)
        {
            startTime = tvShowInfo.ShowStart;
            endTime = tvShowInfo.ShowEnd;
            attributes.MediaTitle = tvShowInfo.ShowName;
            duration = tvShowInfo.Duration;
            attributes.MediaContentType = MediaContentType.TvShow;
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
            MediaArtist = attributes.MediaArtist,
            MediaAlbumName = attributes.MediaAlbumName,
            MediaContentType = attributes.MediaContentType ?? MediaContentType.Unknown
        };
    }

    private record TvContent
    {
        public required string ShowName { get; init; }
        public required DateTimeOffset ShowStart { get; init; }
        public required DateTimeOffset ShowEnd { get; init; }

        public TimeSpan Duration => ShowEnd - ShowStart;
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

    public MediaContentType? MediaContentType { get; set; }

    public decimal? VolumeLevel { get; init; }

    public bool? IsVolumeMuted { get; init; }

    public string? MediaArtist { get; init; }

    public string? MediaAlbumName { get; init; }

    public string? EntityPictureLocal { get; init; }

    public DateTimeOffset? MediaPositionUpdatedAt { get; init; }

    public decimal? MediaDuration { get; init; }

    public string? MediaTitle { get; set; }

    public decimal? MediaPosition { get; init; }
}

public record MediaContent
{
    public required string PlayerEntity { get; init; }

    public string? AppId { get; init; }

    public string? AppName { get; init; }

    public string? MediaArtist { get; init; }

    public string? MediaAlbumName { get; init; }

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