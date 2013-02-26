using System.Web;
using System.Web.Mvc;
using Glimpse.AspNet.Extensions;

namespace Glimpse.Mvc.Html
{
    public static class HtmlHelperExtension
    {
#if MVC2
        public static string GlimpseClient(this HtmlHelper helper)
        {
            var tags = helper.ViewContext.HttpContext.GenerateGlimpseScriptTags();

            return tags;
        }
#else
        public static IHtmlString GlimpseClient(this HtmlHelper helper)
        {
            var tags = helper.ViewContext.HttpContext.GenerateGlimpseScriptTags();

            return helper.Raw(tags);
        }
#endif
    }
}