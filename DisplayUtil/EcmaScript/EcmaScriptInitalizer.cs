using DisplayUtil.Scenes;
using DisplayUtil.Utils;
using Jint;
using Jint.Runtime.Modules;

namespace DisplayUtil.EcmaScript;

public static class EcmaScriptInitializer
{
    public static IHostApplicationBuilder AddEcmaScript(this IHostApplicationBuilder builder)
    {
        var settings = builder.ConfigureAndGet<JsSettings>("JavaScript");

        builder.Services
            .AddSingleton<IModuleLoader, DisplayUtilModuleLoader>()
            .AddScoped<EngineProvider>()
            .AddScoped(s => s.GetRequiredService<EngineProvider>()
                .GetEngine())
            .AddScoped<IScreenProviderSource, EcmaScriptScreenProviderRepo>();

        return builder;
    }


}