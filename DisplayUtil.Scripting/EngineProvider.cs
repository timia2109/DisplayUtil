using DisplayUtil.EcmaScript.Environment;
using Jint;
using Jint.Runtime.Interop;
using Jint.Runtime.Modules;

namespace DisplayUtil.EcmaScript;

public class EngineProvider(
    IModuleLoader moduleLoader,
    IEnumerable<IJsValueProvider> jsValueProviders
)
{
    public Engine GetEngine()
    {
        var options = new Options { };
        options.AllowClr();
        options.Modules.ModuleLoader = moduleLoader;

        var engine = new Engine(options);
        engine.SetValue("log", new Action<object>(Console.WriteLine));
        var exporter = new JsExporter(engine);

        foreach (var provider in jsValueProviders)
        {
            provider.Inject(exporter);
        }

        return engine;
    }

}