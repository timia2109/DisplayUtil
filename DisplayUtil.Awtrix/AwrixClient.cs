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

        public Task SetPowerAsync(bool power)
        {
            return mqttClient.PublishAsync(BuildMessage("power", new { power }));
        }

        public Task SetSleepAsync(int seconds)
        {
            return mqttClient.PublishAsync(BuildMessage("sleep", new { sleep = seconds }));
        }

        public Task PlaySoundAsync(string sound)
        {
            return mqttClient.PublishAsync(BuildMessage("sound", new { sound }));
        }

        public Task PlayRtttlAsync(string rtttl)
        {
            return mqttClient.PublishAsync(BuildMessage("rtttl", rtttl));
        }

        public Task SetMoodlightAsync(AwtrixMoodlight moodlight)
        {
            return mqttClient.PublishAsync(BuildMessage("moodlight", moodlight));
        }

        public Task ClearMoodlightAsync()
        {
            return mqttClient.PublishAsync(BuildMessage("moodlight", string.Empty));
        }

        public Task SetIndicatorAsync(int indicatorNumber, AwtrixIndicator indicator)
        {
            if (indicatorNumber < 1 || indicatorNumber > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(indicatorNumber), "Indicator number must be between 1 and 3");
            }
            return mqttClient.PublishAsync(BuildMessage($"indicator{indicatorNumber}", indicator));
        }

        public Task ClearIndicatorAsync(int indicatorNumber)
        {
            if (indicatorNumber < 1 || indicatorNumber > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(indicatorNumber), "Indicator number must be between 1 and 3");
            }
            return mqttClient.PublishAsync(BuildMessage($"indicator{indicatorNumber}", string.Empty));
        }

        public Task DismissNotificationAsync()
        {
            return mqttClient.PublishAsync(BuildMessage("notify/dismiss", string.Empty));
        }

        public Task NextAppAsync()
        {
            return mqttClient.PublishAsync(BuildMessage("nextapp", string.Empty));
        }

        public Task PreviousAppAsync()
        {
            return mqttClient.PublishAsync(BuildMessage("previousapp", string.Empty));
        }

        public Task SwitchToAppAsync(string appName)
        {
            return mqttClient.PublishAsync(BuildMessage("switch", new { name = appName }));
        }

        public Task RebootAsync()
        {
            return mqttClient.PublishAsync(BuildMessage("reboot", string.Empty));
        }

        public Task DoUpdateAsync()
        {
            return mqttClient.PublishAsync(BuildMessage("doupdate", string.Empty));
        }

        private MqttApplicationMessage BuildMessage<T>(string subTopic, T payload)
        {
            var builder = new MqttApplicationMessageBuilder()
                .WithTopic($"{topic}/{subTopic}")
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);

            // Handle empty payloads - send truly empty payload instead of serialized empty string
            if (payload is string str && string.IsNullOrEmpty(str))
            {
                builder.WithPayload(Array.Empty<byte>());
            }
            else
            {
                builder.WithPayload(Serialize(payload));
            }

            return builder.Build();
        }

        private string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
