using DisplayUtil.EcmaScript.Environment;
using DisplayUtil.XmlModel.Models;

namespace DisplayUtil.EcmaScript;

internal class XmlModelProvider : IJsValueProvider
{
    public void Inject(JsExporter exporter)
    {
        exporter.ExposeNamespaceFunctionsAsCreators<IXmlModel>();
    }
}
