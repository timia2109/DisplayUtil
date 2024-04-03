using DisplayUtil.Template;
using DisplayUtil.Utils;
using NetDaemon.Client.Extensions;
using NetDaemon.Client.Settings;
using NetDaemon.HassModel;

namespace DisplayUtil.HomeAssistant;

public static class HassExtension
{
    public static IHostApplicationBuilder AddHassSupport(this IHostApplicationBuilder builder)
    {
        var settings = builder.ConfigureAndGet<HomeAssistantSettings>(
            "HomeAssistant"
        );

        if (settings is null
            || settings.Host is null
        ) return builder;

        builder.Services
            .AddHomeAssistantClient()
            .AddScoped<ITemplateExtender, HassTemplateExtender>();

        // Hack: Initialize Hass Model
        var extensionType = typeof(DependencyInjectionSetup);
        var methodInfo = extensionType.GetMethod("AddScopedHaContext",
            System.Reflection.BindingFlags.NonPublic
            | System.Reflection.BindingFlags.Static
            )
            ?? throw new Exception("Error injecting Hass Model");

        methodInfo.Invoke(null, [builder.Services]);

        // Background Connection
        builder.Services.AddHostedService<HassHostedService>();

        return builder;
    }
}