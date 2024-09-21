using Jint;
using Jint.Native.Function;
using Quartz;

namespace DisplayUtil.EcmaScript.Jobs;

public class JobExecutor(Engine engine) : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var jobInstance = new JobInstance
        {
            Script = context.MergedJobDataMap.GetString("Script")!,
            ExportedFunctionName = context.MergedJobDataMap.GetString("ExportedFunctionName")
        };

        var module = engine.Modules.Import(jobInstance.Script);
        var exportedFunction = module.Get(jobInstance.ExportedFunctionName ?? "execute");

        if (exportedFunction is not Function callable)
            throw new Exception("Exported member is undefined or not callable");

        callable.Call();

        return Task.CompletedTask;
    }
}