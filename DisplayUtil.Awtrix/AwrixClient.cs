using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DisplayUtil.Awtrix
{
    internal class AwrixClient(IMqttClient mqttClient, string topic) : IAwtrixClient
    {
        public Task SendNotificationAsync(AwtirxNotification notification)
        {
            return mqttClient.PublishAsync(BuildMessage("notification", notification));
        }

        public Task SetAppAsync(string appId, AwtirxCustomApp app)
        {
            return mqttClient.PublishAsync(BuildMessage("custom/" + appId, app));
        }

        public Task SetMatrixStateAsync(bool enabled)
        {
            return mqttClient.PublishAsync(BuildMessage("matrixState", new { enabled }));
        }

        public Task SetSettingsAsync(AwtirxSettings settings)
        {
            return mqttClient.PublishAsync(BuildMessage("settings", settings));
        }

        private MqttApplicationMessage BuildMessage<T>(string subTopic, T payload)
        {
            return new MqttApplicationMessageBuilder()
                .WithTopic($"{topic}/{subTopic}")
                .WithPayload(Serialize(payload))
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();
        }

        private string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
