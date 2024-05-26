using System.Collections.Frozen;
using System.Reflection;
using System.Text.Json;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime;

namespace DisplayUtil.EcmaScript.PropertyObjects;

internal static class PropertyObjectFactory
{
    private static readonly JsonNamingPolicy _namingPolicy = JsonNamingPolicy.CamelCase;

    internal static Function CreateForObject<TTarget>(Engine engine)
    {
        return CreateForObject(typeof(TTarget), engine);
    }

    private static JsProp CreateMapper(FieldInfo fieldInfo, Engine engine)
    {
        var fieldType = fieldInfo.FieldType;

        if (ArrayJsProp.Matches(fieldType))
            return new ArrayJsProp(engine, fieldInfo);

        if (EnumJsProp.Matches(fieldType))
            return new EnumJsProp(engine, fieldInfo);

        return new JsProp(engine, fieldInfo);
    }

    internal static Function CreateForObject(Type type, Engine engine)
    {
        var properties = type
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .ToFrozenDictionary(
                k => _namingPolicy.ConvertName(k.Name),
                v => CreateMapper(v, engine)
            );

        return new PropertyObjectFactoryFunction(
            engine, null, new JsString(type.Name),
            properties, type
        );
    }
}

internal class PropertyObjectFactoryFunction(
    Engine engine,
    Realm? realm,
    JsString? name,
    IReadOnlyDictionary<string, JsProp> properties,
    Type type
) : Function(engine, realm!, name)
{
    protected override JsValue Call(JsValue thisObject, JsValue[] arguments)
    {
        if (arguments.Length == 0) throw new ArgumentException("Require attributes object");

        if (arguments[0] is not ObjectInstance objectInstance)
        {
            throw new ArgumentException("Require attributes object");
        }

        var instance = Activator.CreateInstance(type)!;

        foreach (var (name, prop) in properties)
        {
            var jsValue = objectInstance.Get(name);
            if (jsValue is null) continue;
            prop.SetValue(jsValue, instance);
        }

        return JsValue.FromObject(Engine, instance);
    }
}