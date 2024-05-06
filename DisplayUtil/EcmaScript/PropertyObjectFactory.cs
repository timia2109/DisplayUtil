using System.Reflection;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime;

namespace DisplayUtil.EcmaScript;

internal record JsProp(
    PropertyInfo Target,
    Action<object> SetValue
);

internal class PropertyConstructorFactory
{

    internal Constructor CreateForObject<TTarget>()
    {
        throw new NotImplementedException();
    }

}

internal class PropertyObjectFactory : Function
{
    private readonly IReadOnlyDictionary<string, PropertyInfo> _properties;

    public PropertyObjectFactory(Engine engine, Realm realm, JsString? name, IReadOnlyDictionary<string, PropertyInfo> properties) : base(engine, realm, name)
    {
        _properties = properties;
    }

    protected override JsValue Call(JsValue thisObject, JsValue[] arguments)
    {
        throw new NotImplementedException();
    }
}