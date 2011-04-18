using System.Web;

namespace Glimpse.Net.Extensibility
{
    public interface IGlimpsePlugin
    {
        string Name { get; }
        object GetData(HttpApplication application);
        void SetupInit(HttpApplication application);
    }
}
