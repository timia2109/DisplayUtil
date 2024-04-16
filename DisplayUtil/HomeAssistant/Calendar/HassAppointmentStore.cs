using DisplayUtil.Template;
using Scriban.Runtime;

namespace DisplayUtil.HomeAssistant.Calendar;

public class HassAppointmentStore : ITemplateExtender
{
    public HassEvent[] Appointments { get; set; } = [];

    public void Enrich(ScriptObject scriptObject, EnrichScope scope)
    {
        scriptObject.Add("appointments", Appointments);
    }
}
