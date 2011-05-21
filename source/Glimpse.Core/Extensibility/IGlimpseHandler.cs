using System.Web;

namespace Glimpse.Core.Extensibility
{
    public interface IGlimpseHandler:IHttpHandler
    {
        string ResourceName { get; }
    }
}