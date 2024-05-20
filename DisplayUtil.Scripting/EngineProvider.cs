using DisplayUtil.EcmaScript.Environment;
using Jint;
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
        var exporter = new JsExporter(engine);

        foreach (var provider in jsValueProviders)
        {
            provider.Inject(exporter);
        }

        return engine;
    }

}