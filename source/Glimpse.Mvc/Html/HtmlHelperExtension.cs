using System.Web;
using System.Web.Mvc;
using Glimpse.AspNet;
using Glimpse.Core.Framework;

namespace Glimpse.Mvc.Html
{
    public static class HtmlHelperExtension
    {
#if MVC2
        public static string GlimpseClient(this HtmlHelper helper)
        {
            var tags = string.Empty;

            if (GlimpseRuntime.IsInitialized)
            {
                var aspNetRequestResponseAdapter = GlimpseRuntime.Instance.CurrentRequestContext.RequestResponseAdapter as IAspNetRequestResponseAdapter;
                if (aspNetRequestResponseAdapter != null)
                {
                    tags = aspNetRequestResponseAdapter.GenerateGlimpseScriptTags();
                }
            }

            return tags;
        }
#else
        public static IHtmlString GlimpseClient(this HtmlHelper helper)
        {
            if (GlimpseRuntime.IsInitialized)
            {
                var aspNetRequestResponseAdapter = GlimpseRuntime.Instance.CurrentRequestContext.RequestResponseAdapter as IAspNetRequestResponseAdapter;
                if (aspNetRequestResponseAdapter != null)
                {
                    helper.Raw(aspNetRequestResponseAdapter.GenerateGlimpseScriptTags());
                }
            }

            return helper.Raw(string.Empty);
        }
#endif
    }
}