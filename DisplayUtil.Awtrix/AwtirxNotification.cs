using System.Text.Json.Serialization;

namespace DisplayUtil.Awtrix;

public record AwtirxNotification : AwtirxBase
{
    [JsonPropertyName("hold")] public bool Hold { get; init; } = false;

    [JsonPropertyName("sound")] public string? Sound { get; init; }

    [JsonPropertyName("rtttl")] public string? Rtttl { get; init; }

    [JsonPropertyName("loopSound")] public bool LoopSound { get; init; } = false;

    [JsonPropertyName("stack")] public bool Stack { get; init; } = true;

    [JsonPropertyName("wakeup")] public bool Wakeup { get; init; } = false;

    [JsonPropertyName("clients")] public string[]? Clients { get; init; }
}