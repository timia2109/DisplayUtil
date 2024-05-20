using System.Collections.Frozen;
using System.Reflection;
using System.Text.Json;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime;

namespace DisplayUtil.EcmaScript;

/// <summary>
/// Represents a converted property for an array as target
/// </summary>
internal class ArrayJsProp : JsProp
{
    /// <summary>
    /// Type of the array contents
    /// </summary>
    private readonly Type _itemType;

    public ArrayJsProp(Engine engine, FieldInfo targetProp) : base(engine, targetProp)
    {
        if (!Matches(_targetType))
        {
            throw new Exception("Requires collection");
        }

        _itemType = _targetType.GetElementType()!;
    }

    protected override object? ConvertValue(JsValue jsValue)
    {
        var arr = jsValue.AsArray();
        var items = Array.CreateInstance(_itemType, arr.Length);

        for (var i = 0; i < arr.Length; i++)
        {
            items.SetValue(ConvertValue(arr[i].ToObject()!, _itemType), i);
        }

        return ConvertValue(items, _targetType);
    }

    /// <summary>
    /// Is this property an array?
    /// </summary>
    /// <param name="t">Type of property</param>
    /// <returns>Is array</returns>
    internal static bool Matches(Type t)
    {
        return t.IsArray;
    }
}

/// <summary>
/// Represents a mapped property
/// </summary>
/// <param name="engine">JS Engine</param>
/// <param name="TargetProperty">Target Property</param>
internal class JsProp(
    Engine engine,
    FieldInfo TargetProperty
)
{
    protected Engine CurrentEngine => engine;
    protected readonly Type _targetType = TargetProperty.FieldType;

    /// <summary>
    /// Converts a object to another type
    /// </summary>
    /// <param name="obj">Affected value</param>
    /// <param name="targetType">Target Type</param>
    /// <returns>The converted C# Type</returns>
    protected object? ConvertValue(object obj, Type targetType)
    {
        if (targetType.IsAssignableFrom(obj!.GetType()))
            return obj;

        return Convert.ChangeType(obj, _targetType);
    }

    /// <summary>
    /// Converts the JavaScript value to the <see cref="_targetType"/>
    /// </summary>
    /// <param name="jsValue">JavaScript Value</param>
    /// <returns>Converted value</returns>
    protected virtual object? ConvertValue(JsValue jsValue)
    {
        return ConvertValue(jsValue.ToObject()!, _targetType);
    }

    /// <summary>
    /// Sets the value to the object
    /// </summary>
    /// <param name="jsValue">JavaScript Value</param>
    /// <param name="targetObject">Target Object</param>
    internal void SetValue(
        JsValue jsValue,
        object targetObject
    )
    {
        if (jsValue.IsUndefined() || jsValue.IsNull()) return;

        TargetProperty.SetValue(
            targetObject,
            ConvertValue(jsValue)
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
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .ToFrozenDictionary(
                k => _namingPolicy.ConvertName(k.Name),
                v => ArrayJsProp.Matches(v.FieldType)
                    ? new ArrayJsProp(engine, v)
                    : new JsProp(engine, v)
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