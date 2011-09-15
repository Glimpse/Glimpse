using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Plumbing
{
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

            var contentString = ResponseContent.ToString();
            if (bodyEnd.IsMatch(contentString) && Context.GetGlimpseMode() == GlimpseMode.On)
            {
                var dataPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("data.js") + "&id=" + Context.GetGlimpseRequestId());
                var clientPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("client.js"));

                var html = string.Format(@"<script type='text/javascript' id='glimpseData' src='{0}'></script><script type='text/javascript' id='glimpseClient' src='{1}'></script></body>", dataPath, clientPath);
                
                // Add glimpse output script
                string bodyCloseWithScript = bodyEnd.Replace(contentString,html);

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