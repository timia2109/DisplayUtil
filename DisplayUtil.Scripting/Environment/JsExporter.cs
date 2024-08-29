using DisplayUtil.Scripting.PropertyObjects;
using DisplayUtil.Scripting.PropertyObjects.Mapping;
using Jint;
using Jint.Native;
using Jint.Native.Function;

namespace DisplayUtil.EcmaScript.Environment;

/// <summary>
/// Object to add values to the JavaScript Engine
/// </summary>
internal class JsExporter(Engine engine, MappingRegistry mappingRegistry) : IJsExporter
{
    public void ExposeValue(string variableName, object obj)
    {
        engine.Global[variableName] = JsValue.FromObject(engine, obj);
    }

    public void ExposeFunction(string functionName, Function function)
    {
        engine.Global[functionName] = function;
    }

    public void ExposeFunction(string functionName,
        Func<Engine, Function> factory)
    {
        var function = factory(engine);
        ExposeFunction(functionName, function);
    }

    public void ExposeCreatorFunction<TType>(string functionName)
    {
        ExposeCreatorFunction(typeof(TType), functionName);
    }

    public void ExposeCreatorFunction(Type type, string functionName)
    {
        var mapping = mappingRegistry.RegisterType(type);
        var function = new ObjectArgumentFactoryFunction(engine, null, new JsString(functionName), mapping);
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
            ExposeCreatorFunction(classType, name);
        }
    }

    public void ExposeConverter<TType>()
    {
        /*PropertyObjectFactory.AutoConverters.TryAdd(
            typeof(TType),
            PropertyObjectFactory.CreateForObject<TType>(_engine)
        );*/
    }
}