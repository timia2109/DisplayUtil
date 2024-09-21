using System.Globalization;
using DisplayUtil.EcmaScript;
using DisplayUtil.EcmaScript.Environment;
using DisplayUtil.Scripting.PropertyObjects.Mapping;
using Jint;
using Jint.Runtime.Modules;
using Microsoft.Extensions.Options;

namespace DisplayUtil.Scripting;

/// <summary>
/// Responsible for creating engines with the correct setup
/// </summary>
internal class EngineFactory(IModuleLoader moduleLoader,
    MappingRegistry mappingRegistry,
    IEnumerable<IJsValueProvider> jsValueProviders)
{
    /// <summary>
    /// Creates a new Engine
    /// </summary>
    /// <param name="cultureInfo">Based Culture</param>
    /// <returns>The Engine</returns>
    public Engine CreateEngine(CultureInfo cultureInfo)
    {
        var options = new Jint.Options
        {
            Culture = cultureInfo,
            Strict = true
        };
        options.Modules.ModuleLoader = moduleLoader;

        var engine = new Engine(options);
        engine.SetValue("log", new Action<object>(Console.WriteLine));
        var exporter = new JsExporter(engine, mappingRegistry);

        foreach (var provider in jsValueProviders)
        {
            provider.OnSetup(exporter);
        }

        return engine;
    }
}