using System.Security.Cryptography.X509Certificates;
using MQTTnet;
using MQTTnet.Client;

namespace DisplayUtil.MqttExport;

public static class MqttInitExtension
{
    public static IHostApplicationBuilder AddMqttWriter(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<MqttSettings>(
            builder.Configuration.GetSection("Mqtt"));

        builder.Services.AddScoped<MqttExporter>();

        if (!CreateMqttClient(builder))
        {
            return builder;
        }

        return builder;
    }

    private static bool CreateMqttClient(IHostApplicationBuilder builder)
    {
        var settings = builder.Configuration.GetSection("Mqtt")
            .Get<MqttSettings>();

        if (settings is null || settings.Uri is null)
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
