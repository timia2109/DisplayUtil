using System.Text.Json.Serialization;

namespace DisplayUtil.Awtrix;

public record AwtirxCustomApp : AwtirxBase
{
    [JsonPropertyName("pos")] public int? Position { get; init; }

    [JsonPropertyName("lifetime")] public int Lifetime { get; init; } = 0;

    [JsonPropertyName("lifetimeMode")]
    public AwtirxLifetimeMode LifetimeMode { get; init; } = AwtirxLifetimeMode.Delete;

    [JsonPropertyName("save")] public bool Save { get; init; } = false;
}