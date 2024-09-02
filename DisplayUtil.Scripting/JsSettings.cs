namespace DisplayUtil.EcmaScript;

public record JsSettings
{
    public required IReadOnlyDictionary<string, string> Paths { get; init; }
}
