using System.Collections.Frozen;
using System.Reflection;
using System.Text.Json;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime;

namespace DisplayUtil.EcmaScript;

internal record JsProp(
    PropertyInfo TargetProperty
)
{
    internal void SetValue(
        JsValue jsValue,
        object targetObject
    )
    {
        TargetProperty.SetValue(
            targetObject,
            ((IConvertible)jsValue).ToType(TargetProperty.PropertyType, null)
        );
    }
};

internal static class PropertyObjectFactory
{
    private static readonly JsonNamingPolicy _namingPolicy = JsonNamingPolicy.CamelCase;

    internal static Function CreateForObject<TTarget>(Engine engine, Realm realm)
    {
        return CreateForObject(typeof(TTarget), engine, realm);
    }

    internal static Function CreateForObject(Type type, Engine engine, Realm realm)
    {
        var properties = type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToFrozenDictionary(
                k => _namingPolicy.ConvertName(k.Name),
                v => new JsProp(v)
            );

        return new PropertyObjectFactoryFunction(
            engine, realm, new JsString(type.Name),
            properties, type
        );
    }

    /// <summary>
    /// Adds alls types of the namespace as a function to the engine
    /// </summary>
    /// <typeparam name="TReference">Reference Type of namespace</typeparam>
    /// <param name="engine">Engine</param>
    /// <param name="realm">Realm</param>
    internal static void AddNamespaceMembers<TReference>(
        Engine engine, Realm realm
    )
    {
        var type = typeof(TReference);
        var namespaceName = type.Namespace;

        var requiredTypes = type.Assembly
            .GetTypes()
            .Where(t => t.Namespace == namespaceName)
            .Where(t => t.IsPublic && !t.IsAbstract)
            .ToDictionary(t => t.Name, t => CreateForObject(t, engine, realm));

        foreach (var (name, function) in requiredTypes)
        {
            engine.Global[name] = function;
        }
    }

}

internal class PropertyObjectFactoryFunction(
    Engine engine,
    Realm realm,
    JsString? name,
    IReadOnlyDictionary<string, JsProp> properties,
    Type type
) : Function(engine, realm, name)
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