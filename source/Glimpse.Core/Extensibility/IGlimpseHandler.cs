using System.Web;

namespace Glimpse.Core.Extensibility
{
    public interface IGlimpseHandler
    {
        string ResourceName { get; }
        void ProcessRequest(HttpContextBase context);
    }
}