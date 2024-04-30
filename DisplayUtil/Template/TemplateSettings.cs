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

    /// <summary>
    /// Defines background template jobs
    /// Key = any unique string (key)
    /// Value = definition for the Job
    /// </summary>
    public IReadOnlyDictionary<string, TemplateJobConfiguration> Jobs { get; init; }
        = new Dictionary<string, TemplateJobConfiguration>();
}

public record TemplateJobConfiguration
{
    public string Cron { get; init; } = null!;
    public string TemplateName { get; init; } = null!;
}