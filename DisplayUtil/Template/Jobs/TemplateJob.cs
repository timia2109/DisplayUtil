using Quartz;

namespace DisplayUtil.Template.Jobs;

public class TemplateJob(TemplateRenderer renderer) : IJob
{
    public const string TemplateNameField = "TemplateName";

    public async Task Execute(IJobExecutionContext context)
    {
        var templateNameObject = context.MergedJobDataMap.Get(TemplateNameField);
        if (templateNameObject is not string templateName)
            throw new Exception("Unable to parse Template Name");

        await renderer.EvaluateAsync(templateName);
    }
}