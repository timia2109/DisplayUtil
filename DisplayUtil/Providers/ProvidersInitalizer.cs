using DisplayUtil.Utils;

namespace DisplayUtil.Providers;

public static class ProvidersInitalizer
{

    public static IHostApplicationBuilder AddProviders(this IHostApplicationBuilder builder)
    {
        builder.ConfigureAndGet<ProviderSettings>("Providers");

        builder.Services
            .AddActivatedSingleton(FontProvider.CreateFontProvider)
            .AddSingleton<IconPathProvider>();

        return builder;
    }



}