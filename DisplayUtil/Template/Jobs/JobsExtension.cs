using DisplayUtil.Utils;
using Quartz;

namespace DisplayUtil.Template.Jobs;

internal static class JobsExtension
{
    private const string KeyGroup = nameof(TemplateJob);

    public static void AddTemplateJobs(
        this IHostApplicationBuilder builder,
        TemplateSettings settings
    )
    {
        var jobConfigurations = settings.Jobs;

        builder.Services.Configure<QuartzOptions>(o =>
        {
            foreach (var (key, setting) in jobConfigurations)
            {
                var jobKey = new JobKey(key, KeyGroup);
                o.AddJob<TemplateJob>(j => j
                    .WithIdentity(jobKey)
                    .UsingJobData(TemplateJob.TemplateNameField,
                            setting.TemplateName)
                );

                o.AddTrigger(t => t
                    .WithCronSchedule(setting.Cron)
                    .WithSecurityTimeout()
                    .WithIdentity($"{key}_schedule", KeyGroup)
                );
            }
        });
    }
}