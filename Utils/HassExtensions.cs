using Microsoft.Extensions.Options;
using NetDaemon.Client;
using NetDaemon.Client.Extensions;
using NetDaemon.Client.Settings;
using NetDaemon.HassModel;

namespace DisplayUtil.Utils;

public static class HassExtension
{
    public static IHostApplicationBuilder AddHassSupport(this IHostApplicationBuilder builder)
    {

        builder.Services.Configure<HomeAssistantSettings>(
            builder.Configuration.GetSection("HomeAssistant")
        );

        builder.Services.AddHomeAssistantClient();

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