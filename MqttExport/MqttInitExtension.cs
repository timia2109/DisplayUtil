using System.Security.Cryptography.X509Certificates;
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
            builder.Services.AddScoped<MqttExporter, CachedMqttExporter>();
        else
            builder.Services.AddScoped<MqttExporter>();

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
            .AddSingleton<ExportingMqttClient>();

        return true;
    }
}
