using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public interface IAspNetRequestResponseAdapter : IRequestResponseAdapter
    {
        void PreventSettingHttpResponseHeaders();
    }
}