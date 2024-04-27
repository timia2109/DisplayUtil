using Quartz;

namespace DisplayUtil.Template.Jobs;

internal static class JobsExtension
{
    public static void AddTemplateJobs(
        this IHostApplicationBuilder builder,
        TemplateSettings settings
    )
    {
        var jobConfigurations = settings.Jobs;

        builder.Services.AddQuartz(q =>
        {
            foreach (var (key, setting) in jobConfigurations)
            {
                q.ScheduleJob<TemplateJob>(job =>
                {
                    job.WithIdentity(key)
                        .UsingJobData(TemplateJob.TemplateNameField,
                            setting.TemplateName)
                        .WithCronSchedule(setting.Cron);
                });
            }
        });
    }
}