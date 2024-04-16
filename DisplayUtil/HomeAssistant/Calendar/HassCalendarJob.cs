using DisplayUtil.Utils;

namespace DisplayUtil.HomeAssistant.Calendar;

/// <summary>
/// Job to sync HassAppointment
/// </summary>
internal class HassCalendarJob(
    IServiceScopeFactory scopeFactory,
    ILogger<HassCalendarJob> logger
) : TimedScopedService(scopeFactory, logger)
{
    protected override TimeSpan InitTimeout => TimeSpan.FromSeconds(5);

    protected override TimeSpan Delay => TimeSpan.FromHours(1);

    protected override async Task TriggerAsync(IServiceProvider serviceProvider)
    {
        var worker = serviceProvider.GetRequiredService<HassCalendarWorker>();
        await worker.RefreshAsync();
    }
}
