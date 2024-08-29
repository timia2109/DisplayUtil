using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime;
using DisplayUtil.Scripting.PropertyObjects.Mapping;

namespace DisplayUtil.Scripting.PropertyObjects;

internal class ObjectArgumentFactoryFunction(
    Engine engine,
    Realm? realm,
    JsString? name,
    IMapping mapping
) : Function(engine, realm!, name)
{
    protected override JsValue Call(JsValue thisObject, JsValue[] arguments)
    {
        if (arguments.Length == 0) throw new ArgumentException("Require attributes object");

        if (arguments[0] is not ObjectInstance objectInstance)
        {
            throw new ArgumentException("Require attributes object");
        }

        var instance = mapping.Map(objectInstance);

        return JsValue.FromObject(Engine, instance);
    }
}