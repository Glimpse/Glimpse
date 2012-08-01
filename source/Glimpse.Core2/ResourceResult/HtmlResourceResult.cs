using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.ResourceResult
{
    public class HtmlResourceResult:IResourceResult
    {
        public string Html { get; set; }

        public HtmlResourceResult(string html)
        {
            Html = html;
        }

        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;

            frameworkProvider.SetHttpResponseHeader("Content-Type", "text/html");

            frameworkProvider.SetHttpResponseHeader("Cache-Control", "no-cache");

            frameworkProvider.WriteHttpResponse(Html);
        }
    }
}