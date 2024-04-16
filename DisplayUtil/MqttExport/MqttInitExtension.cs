using DisplayUtil.Utils;
using MQTTnet;
using MQTTnet.Client;

namespace DisplayUtil.MqttExport;

public static class MqttInitExtension
{
    public static IHostApplicationBuilder AddMqttWriter(this IHostApplicationBuilder builder)
    {
        var settings = builder.ConfigureAndGet<MqttSettings>("Mqtt");
        if (settings is null) return builder;

        if (!CreateMqttClient(builder, settings))
            return builder;

        if (settings.IncrementalUpdate)
            builder.Services.AddSingleton<MqttExporter, CachedMqttExporter>();
        else
            builder.Services.AddSingleton<MqttExporter>();

        if (
            settings.ScreenDetectTemplate is null
            || settings.RefreshInterval is null
        ) return builder;

        builder.Services.AddHostedService<MqttExportJob>();

        return builder;
    }

    private static bool CreateMqttClient(
        IHostApplicationBuilder builder,
        MqttSettings settings
    )
    {
        if (settings is null
            || settings.Uri is null
            || settings.ServerHostName is null
            )
            return false;

        var factory = new MqttFactory();

        var client = factory.CreateMqttClient();

        var clientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(settings.Uri)
                .WithClientId("displayutil_" + builder.Environment.EnvironmentName)
                .WithCredentials(settings.User, settings.Password)
                .Build();

        if (client is null || clientOptions is null)
            throw new Exception("Error creating MQTT instances");

        builder.Services
            .AddSingleton(client)
            .AddSingleton(clientOptions)
            .AddSingleton<ExportingMqttClient>()
            .AddScoped<MqttUrlRenderer>();

        return true;
    }

    public static WebApplication UseMqttWriter(this WebApplication app)
    {
        if (app.Services.GetService<MqttExporter>() is null) return app;

        app.MapGet("/mqtt/uri", async (MqttUrlRenderer renderer) =>
        {
            return Results.Ok(await renderer.GetMqttTemplateUriAsync());
        })
        .WithName("Get MQTT URI")
        .WithOpenApi();

        app.MapGet("/mqtt/template", async (MqttUrlRenderer renderer)
            => Results.Ok(await renderer.GetMqttTemplateAsync()))
        .WithName("Get MQTT Template")
        .WithOpenApi();

        return app;
    }
}
