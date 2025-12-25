using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet;

namespace DisplayUtil.Awtrix;

public static class AwtrixInitializer
{
    public static async Task<IHostApplicationBuilder> AddAwtrixAsync(
        this IHostApplicationBuilder builder,
        string configurationSection,
        string? clientName = null
    )
    {
        var configuration = builder.Configuration.GetSection(configurationSection);
        var options = configuration.Get<AwtrixConnectionOptions>() ?? throw new InvalidOperationException(
            $"Awtrix configuration section '{configurationSection}' is missing or invalid."
        );

        var clientFactory = new MqttClientFactory();
        var client = clientFactory.CreateMqttClient();

        var mqttOptionsBuilder = new MqttClientOptionsBuilder()
            .WithTcpServer(options.Host, options.Port);

        if (options.Username != null && options.Password != null)
            mqttOptionsBuilder.WithCredentials(options.Username, options.Password);

        if (options.ClientId != null) mqttOptionsBuilder.WithClientId(options.ClientId);

        await client.ConnectAsync(mqttOptionsBuilder.Build());

        if (clientName != null)
            builder.Services.AddKeyedSingleton<IAwtrixClient>(
                clientName,
                new AwrixClient(client, options.MqttTopic)
            );
        else
            builder.Services.AddSingleton<IAwtrixClient>(
                new AwrixClient(client, options.MqttTopic)
            );

        return builder;
    }
}