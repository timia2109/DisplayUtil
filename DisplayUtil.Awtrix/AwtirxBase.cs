using System.Text.Json;
using System.Text.Json.Serialization;

namespace DisplayUtil.Awtrix;

public abstract record AwtirxBase
{
    [JsonPropertyName("text")] public string? Text { get; init; }

    [JsonPropertyName("textCase")] public AwtirxTextCase TextCase { get; init; } = AwtirxTextCase.Global;

    [JsonPropertyName("topText")] public bool TopText { get; init; } = false;

    [JsonPropertyName("textOffset")] public int TextOffset { get; init; } = 0;

    [JsonPropertyName("center")] public bool Center { get; init; } = true;

    [JsonPropertyName("noScroll")] public bool NoScroll { get; init; } = false;

    [JsonPropertyName("scrollSpeed")] public int ScrollSpeed { get; init; } = 100;

    [JsonPropertyName("color")] public Color? Color { get; init; }

    [JsonPropertyName("gradient")] public Color? Gradient { get; init; }

    [JsonPropertyName("background")] public Color? Background { get; init; }

    [JsonPropertyName("rainbow")] public bool Rainbow { get; init; } = false;

    [JsonPropertyName("blinkText")] public int? BlinkText { get; init; }

    [JsonPropertyName("fadeText")] public int? FadeText { get; init; }

    [JsonPropertyName("icon")] public string? Icon { get; init; }

    [JsonPropertyName("pushIcon")] public AwtirxPushIconMode PushIcon { get; init; } = AwtirxPushIconMode.Static;

    [JsonPropertyName("repeat")] public int Repeat { get; init; } = -1;

    [JsonPropertyName("duration")] public int Duration { get; init; } = 5;

    [JsonPropertyName("bar")] public int[]? Bar { get; init; }

    [JsonPropertyName("line")] public int[]? Line { get; init; }

    [JsonPropertyName("autoscale")] public bool Autoscale { get; init; } = true;

    [JsonPropertyName("barBC")] public Color? BarBackgroundColor { get; init; }

    [JsonPropertyName("progress")] public int Progress { get; init; } = -1;

    [JsonPropertyName("progressC")] public Color? ProgressColor { get; init; }

    [JsonPropertyName("progressBC")] public Color? ProgressBackgroundColor { get; init; }

    [JsonPropertyName("draw")] public Color? Draw { get; init; }

    [JsonPropertyName("effect")] public string? Effect { get; init; }

    [JsonPropertyName("effectSettings")] public Color? EffectSettings { get; init; }

    [JsonPropertyName("overlay")] public string? Overlay { get; init; }
}