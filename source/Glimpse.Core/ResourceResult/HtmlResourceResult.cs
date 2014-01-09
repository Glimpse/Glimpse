using Glimpse.Core.Extensibility;

namespace Glimpse.Core.ResourceResult
{
    /// <summary>
    /// The <see cref="IResourceResult"/> implementation responsible returning Html files to a client.
    /// </summary>
    public class HtmlResourceResult : IResourceResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlResourceResult" /> class.
        /// </summary>
        /// <param name="html">The HTML.</param>
        public HtmlResourceResult(string html)
        {
            Html = html;
        }

        /// <summary>
        /// Gets or sets the Html to send to the client.
        /// </summary>
        /// <value>
        /// The Html.
        /// </value>
        public string Html { get; set; }

        /// <summary>
        /// Executes the resource result with the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.RequestResponseAdapter;

            frameworkProvider.SetHttpResponseHeader("Content-Type", "text/html");

            frameworkProvider.SetHttpResponseHeader("Cache-Control", "no-cache");

            frameworkProvider.WriteHttpResponse(Html);
        }
    }
}