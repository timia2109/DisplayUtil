using Jint;
using Jint.Runtime.Modules;

namespace DisplayUtil.EcmaScript;

internal class ModuleLoader : IModuleLoader
{
    public Module LoadModule(Engine engine, ResolvedSpecifier resolved)
    {
        throw new NotImplementedException();
    }

    public ResolvedSpecifier Resolve(string? referencingModuleLocation, ModuleRequest moduleRequest)
    {
        throw new NotImplementedException();
    }
}