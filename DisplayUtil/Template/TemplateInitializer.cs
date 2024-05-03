using DisplayUtil.Template.Jobs;
using DisplayUtil.Utils;

namespace DisplayUtil.Template;

public static class TemplateInitializer
{
    public static IHostApplicationBuilder AddTemplates(this IHostApplicationBuilder builder)
    {
        var settings = builder.ConfigureAndGet<TemplateSettings>("Templates");

        builder.Services
            .AddSingleton<TemplateLoader>()
            .AddScoped<TemplateRenderer>()
            .AddScoped<TemplateContextProvider>()
            .AddSingleton<ITemplateExtender, UtilTemplateExtender>();

        builder.AddTemplateJobs(settings!);

        return builder;
    }


}