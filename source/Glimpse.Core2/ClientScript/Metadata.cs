using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.ClientScript
{
    public class Metadata:IDynamicClientScript
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.IncludeAfterClientInterfaceScript; }
        }

        public string GetResourceName()
        {
            return Resource.Metadata.InternalName;
        }
    }
}