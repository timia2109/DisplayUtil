using DisplayUtil.EcmaScript.Environment;
using Esprima.Ast;
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

    /// <summary>
    /// Add an <see cref="IJsValueProvider"/> as Scoped to the services
    /// </summary>
    /// <typeparam name="TProvider">Provider</typeparam>
    /// <param name="services">Services</param>
    /// <returns>Services</returns>
    public static IServiceCollection AddScopedJsValueProvider<TProvider>(this IServiceCollection services)
        where TProvider : class, IJsValueProvider
    {
        services.AddScoped<IJsValueProvider, TProvider>();
        return services;
    }

    /// <summary>
    /// Add an <see cref="IJsValueProvider"/> as Singleton to the services
    /// </summary>
    /// <typeparam name="TProvider">Provider</typeparam>
    /// <param name="services">Services</param>
    /// <returns>Services</returns>
    public static IServiceCollection AddSingletonJsValueProvider<TProvider>(this IServiceCollection services)
        where TProvider : class, IJsValueProvider
    {
        services.AddSingleton<IJsValueProvider, TProvider>();
        return services;
    }
}