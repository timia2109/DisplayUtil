using System.ComponentModel.DataAnnotations;

namespace DisplayUtil.EcmaScript.Jobs;

public record JobSettings
{
    /// <summary>
    /// Job Instances
    /// </summary>
    public Dictionary<string, JobInstance>? Jobs { get; init; }
}

/// <summary>
/// Settings for a JobInstance
/// </summary>
public record JobInstance
{
    /// <summary>
    /// Cron expression for the job
    /// </summary>
    [Required] public string Cron { get; init; } = null!;

    /// <summary>
    /// Script to execute
    /// </summary>
    [Required] public string Script { get; init; } = null!;

    /// <summary>
    /// Name of the exported function to call.
    /// Default = execute
    /// </summary>
    public string? ExportedFunctionName { get; init; } = "execute";
}