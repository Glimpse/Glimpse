using System.Web;
using System.Web.Mvc;
using Glimpse.Core;

public static class HtmlExtensions
{
    public static MvcHtmlString GlimpseScript(this HtmlHelper helper)
    {
        var content = "";
        var cookie = helper.ViewContext.RequestContext.HttpContext.Request.Cookies[GlimpseConstants.CookieModeKey];
        if (cookie != null && cookie.Value != "On")
            content = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", VirtualPathUtility.ToAbsolute("~/Glimpse/glimpseClient.js"));
        return MvcHtmlString.Create(content);
    }

    public static MvcHtmlString JQueryScript(this HtmlHelper helper)
    {
        var content = "http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js";
        if (HttpContext.Current.IsDebuggingEnabled)
            content = Links.Scripts.jquery_1_4_4_min_js;
        return MvcHtmlString.Create(string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", content));
    }
}