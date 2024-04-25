using DisplayUtil.Scenes;

namespace DisplayUtil.EcmaScript;

public static class EcmaScriptExtensions
{

    public static IHostApplicationBuilder AddEcmaScript(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IScreenProviderSource,
            EcmaScriptScreenProviderRepo>();
        return builder;
    }

}