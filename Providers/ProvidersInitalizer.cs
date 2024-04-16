using System.Collections.Frozen;
using DisplayUtil.Utils;
using Microsoft.Extensions.Options;

namespace DisplayUtil.Providers;

public static class ProvidersInitalizer
{

    public static IHostApplicationBuilder AddProviders(this IHostApplicationBuilder builder)
    {
        builder.ConfigureAndGet<ProviderSettings>("Providers");

        builder.Services
            .AddSingleton(FontProvider.CreateFontProvider)
            .AddSingleton<IconPathProvider>();

        return builder;
    }



}