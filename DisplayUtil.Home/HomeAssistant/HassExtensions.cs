using DisplayUtil.Home.HomeAssistant;
using DisplayUtil.Home.HomeAssistant.Calendar;
using DisplayUtil.Home.Utils;
using NCronJob;
using NetDaemon.Client.Extensions;
using NetDaemon.Client.Settings;
using NetDaemon.HassModel;
namespace DisplayUtil.Home.HomeAssistant;

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
            .AddScoped<HassUtil>()
            .AddScopedHaContext();

        // Background Connection
        builder.Services.AddHostedService<HassHostedService>();

        var calendarSettings = builder.ConfigureAndGet<HassCalendarSettings>(
            _section
        );
        if (calendarSettings?.CalendarEntities.Length == 0) return builder;

        builder.Services
            .AddScoped<HassCalendarImportJob>();

        builder.Services.AddNCronJob(o =>
        {
            o.AddJob<HassCalendarImportJob>(j => j.WithCronExpression(
                calendarSettings.CalendarCron
            ));
            o.AddJob<HassCalendarImportJob>().RunAtStartup();
        });


        return builder;
    }
}