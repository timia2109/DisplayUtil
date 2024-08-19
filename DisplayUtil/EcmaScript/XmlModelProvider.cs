using DisplayUtil.EcmaScript.Environment;
using DisplayUtil.XmlModel;
using DisplayUtil.XmlModel.Models;

namespace DisplayUtil.EcmaScript;

internal class XmlModelProvider : IJsValueProvider
{
    public void Inject(IJsExporter exporter)
    {
        exporter.ExposeNamespaceFunctionsAsCreators<IXmlModel>();
        exporter.ExposeConverter<DefaultDefinition>();
        exporter.ExposeConverter<XmlSiteSize>();
        // Need to add, it's a record struct
        //exporter.ExposeCreatorFunction<DefaultDefinition>("DefaultDefinition");
    }
}
