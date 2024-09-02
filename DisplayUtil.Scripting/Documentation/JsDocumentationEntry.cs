namespace DisplayUtil.Scripting.Documentation;

public abstract record JsDocumentationEntry
{
    public JsType Type { get; init; }
}