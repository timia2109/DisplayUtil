namespace DisplayUtil.EcmaScript.Jobs;

/// <summary>
/// Payload for Triggering a <see cref="JintJob"/>
/// </summary>
/// <param name="ModuleName">Name of module</param>
/// <param name="ExportedMemberName">Name of Exported Member</param>
public record JintJobPayload(
    string ModuleName,
    string ExportedMemberName = JintJobPayload.DefaultExportedMemberName
)
{
    public const string DefaultExportedMemberName = "onJob";
}