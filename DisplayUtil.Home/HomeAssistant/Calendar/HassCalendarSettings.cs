namespace DisplayUtil.Home.HomeAssistant.Calendar;

public record HassCalendarSettings
{
    public string[] CalendarEntities { get; init; } = [];
    public string CalendarCron { get; init; } = "0 0 * * *";
}
