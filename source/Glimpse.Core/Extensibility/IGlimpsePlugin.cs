using System.Web;

namespace Glimpse.Core.Extensibility
{
    public interface IGlimpsePlugin
    {
        string Name { get; }
        object GetData(HttpApplication application);
        void SetupInit(HttpApplication application);
    }
}
