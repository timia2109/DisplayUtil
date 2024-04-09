using System.Security.Cryptography;
using System.Text.Json.Serialization;
using NetDaemon.Client;
using NetDaemon.Client.HomeAssistant.Model;
using NetDaemon.HassModel.Entities;

namespace DisplayUtil.HomeAssistant;

public class HassCalendarWorker(IHomeAssistantConnection client)
{
    public async Task<HassEvent[]?> FetchAsync(CancellationToken cancellation)
    {
        var command = new GetEventsPayload
        {
            ServiceData = new
            {
                end_date_time = DateTime.Now + TimeSpan.FromDays(7)
            },
            Target = new HassTarget
            {
                EntityIds = [
                    "calendar.jenin_tim",
                    "calendar.it_event",
                    "calendar.th_koln"
                ]
            }
        };

        var response = await client.SendCommandAndReturnResponseAsync
            <GetEventsPayload, GetEventsResponse>(command, cancellation);

        return response?.Events?.ToArray();
    }

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
    [JsonPropertyName("response")] public Dictionary<string, HassEvents> Response { get; init; }

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