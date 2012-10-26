using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;

namespace Glimpse.Core.ClientScript
{
    public class Client : IDynamicClientScript
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.ClientInterfaceScript; }
        }

        public string GetResourceName()
        {
            return ClientResource.InternalName;
        }
    }
}