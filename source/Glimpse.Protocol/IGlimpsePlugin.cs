using System.Web;

namespace Glimpse.Protocol
{
    public interface IGlimpsePlugin
    {
        string Name { get; }
        object GetData(HttpApplication application);
        void SetupInit();
    }
}
