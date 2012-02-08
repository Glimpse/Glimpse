using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.ClientScript
{
    public class Client:IDynamicClientScript
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.ClientInterfaceScript; }
        }

        public string GetResourceName()
        {
            return Resource.Client.InternalName;
        }
    }
}