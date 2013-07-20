using Glimpse.Core.Extensibility;

namespace MvcMusicStore.Framework
{
    public class QueryClientScript : IStaticClientScript
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.IncludeAfterClientInterfaceScript; }
        }

        public string GetUri(string version)
        {
            return "/Framework/QueryScript.js";
        }
    }
}