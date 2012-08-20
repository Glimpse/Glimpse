using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;

namespace Glimpse.Core.ClientScript
{
    public class Metadata:IDynamicClientScript, IParameterValueProvider
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.RequestMetadataScript; }
        }

        public string GetResourceName()
        {
            return MetadataResource.InternalName;
        }

        public void OverrideParameterValues(IDictionary<string, string> defaults)
        {
            defaults[ResourceParameter.Callback.Name] = "glimpse.data.initMetadata";
        }
    }
}