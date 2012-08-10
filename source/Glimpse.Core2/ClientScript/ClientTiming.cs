using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.ClientScript
{
    public class ClientTiming : IDynamicClientScript
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.IncludeAfterRequestDataScript; }
        }

        public string GetResourceName()
        {
            return Resource.ClientTimingResource.InternalName;
        }
    }
}