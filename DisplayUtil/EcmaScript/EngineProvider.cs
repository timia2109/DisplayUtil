using DisplayUtil.XmlModel.Models;
using Jint;
using Jint.Native;
using Jint.Runtime.Modules;

namespace DisplayUtil.EcmaScript;

public class EngineProvider(
    IModuleLoader moduleLoader
)
{
    public Engine GetEngine()
    {
        var options = new Options { };
        options.AllowClr();
        options.Modules.ModuleLoader = moduleLoader;

        var engine = new Engine(options);
        PropertyObjectFactory.AddNamespaceMembers<IXmlModel>(
            engine,
            new Jint.Runtime.Realm()
        );

        return engine;
    }

}