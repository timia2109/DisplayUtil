namespace DisplayUtil.Template;

public record TemplateSettings
{
    /// <summary>
    /// Defines the pathes for templates
    /// Key = any unique string (only for merging)
    /// Value = Path to templates
    /// </summary>
    public IReadOnlyDictionary<string, string> Paths { get; init; }
        = new Dictionary<string, string>();

}