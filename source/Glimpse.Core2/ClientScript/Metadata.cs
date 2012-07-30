using System.Collections.Generic;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.ClientScript
{
    public class Metadata:IDynamicClientScript, IParameterValueProvider
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.RequestMetadataScript; }
        }

        public string GetResourceName()
        {
            return Resource.Metadata.InternalName;
        }

        public void OverrideParameterValues(IDictionary<string, string> defaults)
        {
            defaults[ResourceParameter.Callback.Name] = "glimpse.data.initMetadata";
        }
    }
}