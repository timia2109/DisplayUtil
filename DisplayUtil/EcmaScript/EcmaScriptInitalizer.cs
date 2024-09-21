using DisplayUtil.EcmaScript.Environment;
using DisplayUtil.EcmaScript.Jobs;
using DisplayUtil.Scenes;
using DisplayUtil.Utils;
using Quartz;

namespace DisplayUtil.EcmaScript;

public static class EcmaScriptInitializer
{
    public static IHostApplicationBuilder AddEcmaScript(this IHostApplicationBuilder builder)
    {
        var settings = builder.ConfigureAndGet<JsSettings>("JavaScript");

        builder.Services.AddScripting();

        builder.Services
            .AddScoped<IScreenProviderSource, EcmaScriptScreenProviderRepo>()
            .AddSingleton<IJsValueProvider, XmlModelProvider>();

        AddJobs(builder);

        return builder;
    }

    private static void AddJobs(this IHostApplicationBuilder builder)
    {
        var settings = builder.ConfigureAndGet<JobSettings>("JavaScript");

        if (settings?.Jobs is null)
            return;

        builder.Services.Configure<QuartzOptions>(o =>
        {
            foreach (var (key, instance) in settings.Jobs)
            {
                var jobKey = new JobKey(key);

                o.AddJob<JobExecutor>(o => o
                    .WithIdentity(jobKey)
                    .UsingJobData("Script", instance.Script)
                    .UsingJobData("ExportedFunctionName", instance.ExportedFunctionName)
                );
                o.AddTrigger(t => t
                    .ForJob(jobKey)
                    .WithSecurityTimeout()
                    .WithCronSchedule(instance.Cron)
                );
            }
        });
    }
}