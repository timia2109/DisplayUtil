using Jint;
using Jint.Native.Function;
using NCronJob;

namespace DisplayUtil.EcmaScript.Jobs;

internal partial class JintJob(ILogger<JintJob> logger, Engine engine) : IJob
{
    private readonly ILogger<JintJob> _logger = logger;

    public async Task RunAsync(JobExecutionContext context, CancellationToken token)
    {
        if (context.Parameter is not JintJobPayload payload)
        {
            LogCanNotFindJobPayload();
            return;
        }

        var module = engine.Modules.Import(payload.ModuleName);
        var function = module.Get(payload.ExportedMemberName) as Function;
        if (function == null)
        {
            LogCanNotFindExportedMember(payload.ModuleName, payload.ExportedMemberName);
            return;
        }

        function.Call();
    }

    [LoggerMessage(LogLevel.Error, "Can not find JobPayload")]
    private partial void LogCanNotFindJobPayload();

    [LoggerMessage(LogLevel.Error, "Can not find exported member {member} of {ModuleName}")]
    private partial void LogCanNotFindExportedMember(string ModuleName, string member);
}