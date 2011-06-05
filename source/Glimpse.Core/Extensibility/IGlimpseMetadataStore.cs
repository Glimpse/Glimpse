using System.Web;

namespace Glimpse.Core.Extensibility
{
    public interface IGlimpseMetadataStore
    {
        void Persist(string json, HttpContextBase context);
    }
}
