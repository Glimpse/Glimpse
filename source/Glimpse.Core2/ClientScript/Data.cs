using System.Collections.Generic;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.ClientScript
{
    public class Data:IDynamicClientScript, IParameterValueProvider
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.RequestDataScript; }
        }

        public string GetResourceName()
        {
            return Resource.RequestResource.InternalName;
        }

        public void OverrideParameterValues(IDictionary<string, string> defaults)
        {
            defaults[ResourceParameter.Callback.Name] = "glimpse.data.initData";
        }
    }
}