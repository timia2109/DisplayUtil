using DisplayUtil.EcmaScript;
using DisplayUtil.HomeAssistant.Calendar;
using DisplayUtil.Template;
using DisplayUtil.Utils;
using NetDaemon.Client.Extensions;
using NetDaemon.Client.Settings;
using NetDaemon.HassModel;
using Quartz;

namespace DisplayUtil.HomeAssistant;

public static class HassExtension
{
    private const string _section = "HomeAssistant";

    public static IHostApplicationBuilder AddHassSupport(this IHostApplicationBuilder builder)
    {
        var settings = builder.ConfigureAndGet<HomeAssistantSettings>(_section);

        if (settings is null
            || settings.Host is null
        ) return builder;

        builder.Services
            .AddHomeAssistantClient()
            .AddScoped<ITemplateExtender, HassTemplateExtender>()
            .AddScopedJsValueProvider<HassTemplateExtender>()
            .AddScopedHaContext();

        // Background Connection
        builder.Services.AddHostedService<HassHostedService>();

        var calendarSettings = builder.ConfigureAndGet<HassCalendarSettings>(
            _section
        );
        if (calendarSettings?.CalendarEntities.Length == 0) return builder;

        builder.Services
            .AddSingleton<HassAppointmentStore>()
            .AddSingleton<ITemplateExtender>(s => s.GetRequiredService<HassAppointmentStore>())
            .AddScoped<HassCalendarImportJob>();

        builder.Services.Configure<QuartzOptions>(o =>
        {
            var jobKey = new JobKey(nameof(HassCalendarImportJob));
            o.AddJob<HassCalendarImportJob>(j => j.WithIdentity(jobKey));

            o.AddTrigger(t => t
                .ForJob(jobKey)
                .WithSecurityTimeout()
                .WithSimpleSchedule(s => s.WithIntervalInHours(1))
            );
        });

        return builder;
    }
}