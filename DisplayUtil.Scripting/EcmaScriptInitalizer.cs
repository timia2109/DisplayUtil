using Jint.Runtime.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace DisplayUtil.EcmaScript;

public static class EcmaScriptInitializer
{
    /// <summary>
    /// Adds the Scripting resources. It assumes that the <see cref="JsSettings"/>
    /// are already injected
    /// </summary>
    /// <param name="services">Services</param>
    /// <returns>Services</returns>
    public static IServiceCollection AddScripting(this IServiceCollection services)
    {
        services
            .AddSingleton<IModuleLoader, DisplayUtilModuleLoader>()
            .AddScoped<EngineProvider>()
            .AddScoped(s => s.GetRequiredService<EngineProvider>()
                .GetEngine());

        return services;
    }


}