using DisplayUtil.EcmaScript.Environment;
using DisplayUtil.Scenes;
using DisplayUtil.Utils;

namespace DisplayUtil.EcmaScript;

public static class EcmaScriptInitializer
{
    public static IHostApplicationBuilder AddEcmaScript(this IHostApplicationBuilder builder)
    {
        var settings = builder.ConfigureAndGet<JsSettings>("JavaScript");

        builder.Services
            .AddScoped<IScreenProviderSource, EcmaScriptScreenProviderRepo>()
            .AddSingleton<IJsValueProvider, XmlModelProvider>();

        return builder;
    }
}