
using Microsoft.Extensions.Options;
using NetDaemon.Client;
using NetDaemon.Client.HomeAssistant.Model;
using NetDaemon.Client.Settings;
using NetDaemon.HassModel;
using System.Text.Json;

namespace DisplayUtil.HomeAssistant.Registration;

internal partial class HaStateSubscriber(
    IHomeAssistantRunner runner,
    IOptions<HomeAssistantSettings> options,
    ILogger<HaStateSubscriber> logger,
    HaStateRegistry haStateRegistry) : IHostedService, IDisposable
{
    private readonly ILogger _logger = logger;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private IDisposable? _connectionSubscription;
    private IObservable<HassEvent>? _stateSubscription;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var haSettings = options.Value;
        _connectionSubscription = runner.OnConnect.SubscribeAsync(HandleRegistration);
        _ = runner.RunAsync(
             haSettings.Host,
             haSettings.Port,
             haSettings.Ssl,
             haSettings.Token,
             TimeSpan.FromSeconds(10),
             _cancellationTokenSource.Token
        );
    }

    private async Task HandleRegistration(IHomeAssistantConnection connection)
    {
        if (_cancellationTokenSource.IsCancellationRequested) return;

        _stateSubscription = await connection.SubscribeToHomeAssistantEventsAsync(
            "state_changed", _cancellationTokenSource.Token);
        _stateSubscription.Subscribe(HandleStateChange);
    }

    private void HandleStateChange(HassEvent hassEvent)
    {
        if (hassEvent.EventType != "state_changed") return;

        var entityId = hassEvent.DataElement?.GetProperty("entity_id").GetString()!;
        var registration = haStateRegistry.GetRegistration(entityId);
        if (registration is null)
        {
            LogIgnoringStateChange(entityId);
            return;
        }

        LogHandlingStateChange(entityId);
        var newStateElement = hassEvent.DataElement?.GetProperty("new_state");
        registration.LatestState = newStateElement?.Deserialize<HassState>();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _connectionSubscription?.Dispose();
    }

    [LoggerMessage(LogLevel.Debug, "Ignoring State Change for {entityId}")]
    private partial void LogIgnoringStateChange(string entityId);

    [LoggerMessage(LogLevel.Debug, "Handling State Change for {entityId}")]
    private partial void LogHandlingStateChange(string entityId);
}