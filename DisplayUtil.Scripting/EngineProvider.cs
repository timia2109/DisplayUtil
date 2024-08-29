using System.Globalization;
using DisplayUtil.EcmaScript.Environment;
using DisplayUtil.Scripting.PropertyObjects.Mapping;
using Jint;
using Jint.Runtime.Interop;
using Jint.Runtime.Modules;

namespace DisplayUtil.EcmaScript;

public class EngineProvider(
    IModuleLoader moduleLoader,
    MappingRegistry mappingRegistry,
    IEnumerable<IJsValueProvider> jsValueProviders
)
{
    public Engine GetEngine(CultureInfo cultureInfo)
    {
        var options = new Options { };
        options.AllowClr();
        options.Culture = cultureInfo;
        options.Strict = true;
        options.Modules.ModuleLoader = moduleLoader;

        var engine = new Engine(options);
        engine.SetValue("log", new Action<object>(Console.WriteLine));
        var exporter = new JsExporter(engine, mappingRegistry);

        foreach (var provider in jsValueProviders)
        {
            provider.Inject(exporter);
        }

        return engine;
    }

}