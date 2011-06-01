using System.Web;

namespace Glimpse.Core.Extensibility
{
    public interface IGlimpsePlugin
    {
        string Name { get; }
        object GetData(HttpContextBase context);
        void SetupInit();
    }
}
