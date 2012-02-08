using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.ClientScript
{
    public class Data:IDynamicClientScript
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.RequestDataScript; }
        }

        public string GetResourceName()
        {
            return Resource.Data.InternalName;
        }
    }
}