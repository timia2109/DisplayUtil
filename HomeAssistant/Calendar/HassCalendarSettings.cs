namespace DisplayUtil.HomeAssistant.Calendar;

public record HassCalendarSettings
{
    public string[] CalendarEntities { get; init; } = [];
}
