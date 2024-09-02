using DisplayUtil.EcmaScript.Environment;
using DisplayUtil.XmlModel;
using DisplayUtil.XmlModel.Models;

namespace DisplayUtil.EcmaScript;

internal class XmlModelProvider : IJsValueProvider
{
    public void AfterSetup()
    {
    }

    public void OnDispose()
    {
    }

    public void OnSetup(IJsExporter exporter)
    {
        exporter.ExposeNamespaceFunctionsAsCreators<IXmlModel>();
        exporter.ExposeConverter<DefaultDefinition>();
        exporter.ExposeConverter<XmlSiteSize>();
    }
}
