using DisplayUtil.EcmaScript.PropertyObjects;
using Jint;
using Jint.Native;
using Jint.Native.Function;

namespace DisplayUtil.EcmaScript.Environment;

/// <summary>
/// Object to add values to the JavaScript Engine
/// </summary>
internal class JsExporter : IJsExporter
{
    private readonly Engine _engine;

    internal JsExporter(Engine engine)
    {
        _engine = engine;
    }

    public void ExposeValue(string variableName, object obj)
    {
        _engine.Global[variableName] = JsValue.FromObject(_engine, obj);
    }

    public void ExposeFunction(string functionName, Function function)
    {
        _engine.Global[functionName] = function;
    }

    public void ExposeFunction(string functionName,
        Func<Engine, Function> factory)
    {
        var function = factory(_engine);
        ExposeFunction(functionName, function);
    }

    public void ExposeCreatorFunction<TType>(string functionName)
    {
        var function = PropertyObjectFactory.CreateForObject<TType>(_engine);
        ExposeFunction(functionName, function);
    }

    public void ExposeNamespaceFunctionsAsCreators<TRefType>()
    {
        var type = typeof(TRefType);
        var namespaceName = type.Namespace;

        var requiredTypes = type.Assembly
            .GetTypes()
            .Where(t => t.Namespace == namespaceName)
            .Where(t => t.IsPublic && !t.IsAbstract)
            .ToDictionary(t => t.Name);

        foreach (var (name, classType) in requiredTypes)
        {
            var function = PropertyObjectFactory.CreateForObject(classType, _engine);
            ExposeFunction(name, function);
        }
    }
}