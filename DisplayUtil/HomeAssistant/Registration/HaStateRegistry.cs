using NetDaemon.Client;
using NetDaemon.Client.HomeAssistant.Extensions;

namespace DisplayUtil.HomeAssistant.Registration;

public partial class HaStateRegistry(IHomeAssistantRunner runner, ILogger<HaStateRegistry> logger)
{
    private readonly ILogger _logger = logger;

    public Dictionary<string, HassRegistration> Registrations { get; } = [];

    public void UnregisterState(string entityId)
    {
        LogRemoveState(entityId);
        Registrations.Remove(entityId);
    }

    public HassRegistration RegisterState(string entityId)
    {
        LogAddState(entityId);
        var registration = new HassRegistration();
        if (!Registrations.TryAdd(entityId, registration))
        {
            LogEntityAlreadyRegistered(entityId);
            return Registrations[entityId];
        }
        _ = FetchStateAsync(entityId, registration);
        return registration;
    }

    private async Task FetchStateAsync(string entityId, HassRegistration registration)
    {
        while (runner.CurrentConnection == null)
        {
            await Task.Delay(50);
        }

        var state = await runner.CurrentConnection.GetEntityStateAsync(entityId, CancellationToken.None);
        registration.LatestState = state;
    }

    internal HassRegistration? GetRegistration(string entityId)
    {
        if (Registrations.TryGetValue(entityId, out var registration))
        {
            return registration;
        }

        return null;
    }

    [LoggerMessage(LogLevel.Information, "Entity {entityId} already registered")]
    private partial void LogEntityAlreadyRegistered(string entityId);

    [LoggerMessage(LogLevel.Information, "Removing state {entityId}")]
    private partial void LogRemoveState(string entityId);

    [LoggerMessage(LogLevel.Information, "Adding state {entityId}")]
    private partial void LogAddState(string entityId);

}
