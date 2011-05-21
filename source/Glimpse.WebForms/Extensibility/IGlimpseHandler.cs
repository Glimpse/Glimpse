using System.Web;

namespace Glimpse.WebForms.Extensibility
{
    public interface IGlimpseHandler:IHttpHandler
    {
        string ResourceName { get; }
    }
}