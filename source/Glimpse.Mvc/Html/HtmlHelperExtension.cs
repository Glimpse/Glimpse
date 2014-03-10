using System.Web;
using System.Web.Mvc;
using Glimpse.Core.Framework;

namespace Glimpse.Mvc.Html
{
    public static class HtmlHelperExtension
    {
#if MVC2
        public static string GlimpseClient(this HtmlHelper helper)
        {
            return GlimpseRuntime.IsInitialized
                ? GlimpseRuntime.Instance.GenerateScriptTags(GlimpseRuntime.Instance.CurrentRequestContext)
                : string.Empty;
        }
#else
        public static IHtmlString GlimpseClient(this HtmlHelper helper)
        {
            return helper.Raw( 
                GlimpseRuntime.IsInitialized 
                    ? GlimpseRuntime.Instance.GenerateScriptTags(GlimpseRuntime.Instance.CurrentRequestContext) 
                    : string.Empty);
        }
#endif
    }
}