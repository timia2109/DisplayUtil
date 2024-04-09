using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using NetDaemon.Client;
using NetDaemon.Client.HomeAssistant.Model;

namespace DisplayUtil.HomeAssistant.Calendar;

public partial class HassCalendarWorker(
    IHomeAssistantConnection client,
    HassAppointmentStore store,
    IOptions<HassCalendarSettings> options,
    ILogger<HassCalendarWorker> logger
)
{
    private readonly ILogger _logger = logger;

    public async Task RefreshAsync()
    {
        var appointments = await FetchAsync(CancellationToken.None);
        store.Appointments = appointments ?? [];
    }

    public async Task<HassEvent[]?> FetchAsync(CancellationToken cancellation)
    {
        LogFetch();

        if (client is null)
        {
            LogNoConnection();
            return null;
        }

        var command = new GetEventsPayload
        {
            ServiceData = new
            {
                end_date_time = DateTime.Now + TimeSpan.FromDays(7)
            },
            Target = new HassTarget
            {
                EntityIds = options.Value.CalendarEntities
            }
        };

        try
        {
            var response = await client.SendCommandAndReturnResponseAsync
                <GetEventsPayload, GetEventsResponse>(command, cancellation);

            return response?.Events?.ToArray();
        }
        catch (Exception e)
        {
            LogError(e);
            return null;
        }
    }

    [LoggerMessage(LogLevel.Information, "Fetching Appointments")]
    private partial void LogFetch();

    [LoggerMessage(LogLevel.Warning, "No HASS Connection")]
    private partial void LogNoConnection();

    [LoggerMessage(LogLevel.Warning, "Error fetching appointments")]
    private partial void LogError(Exception e);

}

internal record GetEventsPayload : CommandMessage
{
    public GetEventsPayload()
    {
        Type = "call_service";
    }

    [JsonPropertyName("domain")] public string Domain => "calendar";

    [JsonPropertyName("service")] public string Service => "get_events";

    [JsonPropertyName("service_data")] public object? ServiceData { get; init; }

    [JsonPropertyName("target")] public HassTarget? Target { get; init; }

    [JsonPropertyName("return_response")] public bool ReturnResponse => true;
}

internal record GetEventsResponse : CommandMessage
{
    [JsonPropertyName("response")] public Dictionary<string, HassEvents> Response { get; init; } = null!;

    public IEnumerable<HassEvent> Events => Response.Values
        .SelectMany(e => e.Event)
        .OrderBy(e => e.Start);
}

public record HassEvents
{
    [JsonPropertyName("events")] public HassEvent[] Event { get; init; } = null!;
}

public record HassEvent
{
    [JsonPropertyName("start")] public DateTime Start { get; init; }
    [JsonPropertyName("end")] public DateTime End { get; init; }
    [JsonPropertyName("summary")] public string Summary { get; init; } = null!;
}