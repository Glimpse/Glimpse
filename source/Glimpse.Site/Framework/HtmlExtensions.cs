using System;
using System.Web;
using System.Web.Mvc;
using Glimpse.Core;

public static class HtmlExtensions
{
    public static MvcHtmlString GlimpseScript(this HtmlHelper helper)
    {
        var content = "";
        var cookie = helper.ViewContext.RequestContext.HttpContext.Request.Cookies[GlimpseConstants.CookieModeKey];
        if (cookie == null || cookie.Value != "On")
            content = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", VirtualPathUtility.ToAbsolute("~/Glimpse.axd?r=client.js"));
        return MvcHtmlString.Create(content);
    }

    public static MvcHtmlString JQueryScript(this HtmlHelper helper)
    {
        var content = "http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js";
        if (HttpContext.Current.IsDebuggingEnabled)
            content = Links.Scripts.jquery_1_4_4_min_js;
        return MvcHtmlString.Create(string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", content));
    }

    public static MvcHtmlString GlimpseDownload(this HtmlHelper helper, bool showFull)
    {
        var result = "";
        if (showFull)
            result = string.Format(@" 
                <div class=""download"">
                    <table width=""100%"">
                        <tr>
                            <td  width=""300"">
                                <h3>How to get Glimpse</h3>
                                <div>Currently ASP.Net build only</div>
                            </td>
                            <td width=""250"">
                                <a href=""http://nuget.org/List/Packages/Glimpse"" class=""myButton"">Download Now</a>
                            </td>
                            <td>
                                <a href=""http://nuget.org/List/Packages/Glimpse""><img src=""{0}"" /></a>
                            </td>
                            <td align=""right""> 
                                <a href=""http://twitter.com/share"" class=""twitter-share-button"" data-url=""http://getglimpse.com"" data-text=""Found #glimpse & think its amazing, what firebug is for the client, #glimpse is for the server"" data-count=""horizontal"" data-via=""anthony_vdh @nikmd23"" >Tweet</a>
                                <script type=""text/javascript"" src=""http://platform.twitter.com/widgets.js""></script>
                            </td>
                        </tr>
                    </table>
                </div>
            ", Links.Content.nugetlogo_png);
        else
            result = string.Format(@" 
                <div class=""download-abbrv"">
                    <table>
                        <tr>
                            <td width=""250"">
                                <a href=""http://nuget.org/List/Packages/Glimpse"" class=""myButton"">Download Now</a>
                            </td>
                            <td>
                                <a href=""http://nuget.org/List/Packages/Glimpse""><img src=""{0}"" /></a>
                            </td>
                        </tr>
                    </table>
                </div>
            ", Links.Content.nugetlogo_png);
        return MvcHtmlString.Create(result);
    }

    public static MvcHtmlString GlimpseLinks(this HtmlHelper helper)
    {
        var result = string.Format(@"  
            <div class=""footer"">
                <img src=""{0}"" width=""16"" /> Have a <em>feature</em> request? <a href=""http://getglimpse.uservoice.com"">Submit the idea</a>. &nbsp; &nbsp; 
                <img src=""{1}"" /> Found an <em>error</em>? <a href=""https://github.com/glimpse/glimpse/issues"">Help us improve</a>. &nbsp; &nbsp;
                <img src=""{2}"" /> Have a <em>question</em>? <a href=""http://twitter.com/#search?q=%23glimpse"">Tweet us using #glimpse</a>.
            </div> 
        ", Links.Content.uservoice_icon_png, Links.Content.github_gif, Links.Content.twitter_png);
        return MvcHtmlString.Create(result);
    }
}