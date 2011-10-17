using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Glimpse.Core.Plumbing
{
    using Glimpse.Core.Configuration;
    using Glimpse.Core.Extensions;

    //Heavily influenced by http://www.4guysfromrolla.com/articles/120308-1.aspx
    public class GlimpseResponseFilter : MemoryStream
    {
        internal StringBuilder ResponseContent { get; set; }

        internal Stream OutputStream { get; set; }

        internal HttpContextBase Context { get; set; }

        private Regex bodyEnd = new Regex("</body>", RegexOptions.Compiled | RegexOptions.Multiline);

        public GlimpseResponseFilter(Stream output, HttpContextBase context)
        {
            ResponseContent = new StringBuilder();
            OutputStream = output;
            Context = context;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // Convert the content in buffer to a string
            string contentInBuffer = UTF8Encoding.UTF8.GetString(buffer);

            // Buffer content in responseContent until we reach the end of the page's markup
            ResponseContent.Append(contentInBuffer);

            if (this.Context.GetGlimpseMode() == GlimpseMode.On && this.bodyEnd.IsMatch(contentInBuffer))
            {
                var dataResourcePath = this.Context.GlimpseResourcePath("data.js") + "&id=" + Context.GetGlimpseRequestId();
                var clientScriptResourcePath = this.Context.GlimpseResourcePath("client.js");

                if (this.Context.Response.ContentType == "application/xhtml+xml")
                {
                    // Convert application/xhtml+xml safe '&' to URL
                    dataResourcePath = dataResourcePath.Replace("&amp;", "&");
                    clientScriptResourcePath = clientScriptResourcePath.Replace("&amp;", "&");

                    dataResourcePath = dataResourcePath + "&xhtml=true";
                }

                var dataPath = HttpUtility.HtmlAttributeEncode(dataResourcePath);
                var clientPath = HttpUtility.HtmlAttributeEncode(clientScriptResourcePath);

                var html =
                    string.Format(
                        @"<script type='text/javascript' id='glimpseData' data-glimpse-requestID='{2}' src='{0}'></script><script type='text/javascript' id='glimpseClient' src='{1}'></script></body>",
                        dataPath,
                        clientPath,
                        Context.GetGlimpseRequestId());

                // Add glimpse output script
                string bodyCloseWithScript = bodyEnd.Replace(contentInBuffer, html);

                // Write content to the outputStream
                byte[] outputBuffer = UTF8Encoding.UTF8.GetBytes(bodyCloseWithScript);

                OutputStream.Write(outputBuffer, 0, outputBuffer.Length);
            }
            else
            {
                OutputStream.Write(buffer, offset, count);
            }
        }
    }
}