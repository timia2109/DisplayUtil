using DisplayUtil.Utils;

namespace DisplayUtil.Template;

public static class TemplateInitializer
{
    public static IHostApplicationBuilder AddTemplates(this IHostApplicationBuilder builder)
    {
        builder.ConfigureAndGet<TemplateSettings>("Templates");

        builder.Services
            .AddSingleton<TemplateLoader>()
            .AddScoped<TemplateRenderer>()
            .AddScoped<TemplateContextProvider>()
            .AddSingleton<ITemplateExtender, UtilTemplateExtender>();

        return builder;
    }


}