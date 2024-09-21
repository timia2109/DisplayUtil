using DisplayUtil.EcmaScript.Environment;

namespace DisplayUtil.HomeAssistant.Calendar;

public class HassAppointmentStore : IJsValueProvider
{
    public HassEvent[] Appointments { get; set; } = [];

    public void OnSetup(IJsExporter exporter)
    {
        exporter.ExposeValue("appointments", Appointments);
    }
}
