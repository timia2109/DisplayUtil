using System.Text.Json;
using System.Text.Json.Serialization;
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

    public const string JsonKey = "home-assistant";

    public static IHostApplicationBuilder AddHassSupport(this IHostApplicationBuilder builder)
    {
        var settings = builder.ConfigureAndGet<HomeAssistantSettings>(_section);

        if (settings is null
            || settings.Host is null
           ) return builder;

        builder.Services.AddKeyedSingleton(JsonKey,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower)
                }
            });

        builder.Services
            .AddHomeAssistantClient()
            .AddScoped<HassUtil>()
            .AddScoped<MediaPlayerService>()
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